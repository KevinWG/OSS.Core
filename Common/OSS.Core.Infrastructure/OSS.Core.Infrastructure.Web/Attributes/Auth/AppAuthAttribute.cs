using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context;
using OSS.Core.Infrastructure.Helpers;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;
using OSS.Core.Infrastructure.Web.Helpers;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth
{
    /// <summary>
    ///  应用授权过滤器
    /// </summary>
    public class AppAuthAttribute: BaseOrderAuthAttribute
    {
        private readonly AppAuthOption _appOption;
        
        /// <summary>
        ///  构造函数
        /// </summary>
        public AppAuthAttribute():this(null)
        {
        }

        /// <summary>
        ///  构造函数
        /// </summary>
        public AppAuthAttribute(AppAuthOption appOption)
        {
            p_Order = -1000;
            _appOption = appOption;
        }

        /// <summary>
        ///  应用的授权判断处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // 0.  获取初始化app信息
            var appInfo = AppWebInfoHelper.GetOrSetAppIdentity(context.HttpContext); 
            if (appInfo==null)
            {
                ResponseExceptionEnd(context, new Resp(SysRespTypes.AppConfigError, $"请使用{nameof(InitialMiddleware)}中间件初始化全局上下文信息"));
                return;
            }

            appInfo.SourceMode = GetAppSourceMode(context.HttpContext, _appOption);

            // 1. app验证
            var res = await FormatAndCheck(context.HttpContext, appInfo, _appOption);
            if (!res.IsSuccess())
            {
                ResponseExceptionEnd(context,res);
                return;
            }

            //2. Tenant 验证
            res = await TenantFormatAndCheck(context.HttpContext, appInfo, _appOption);
            if (!res.IsSuccess())
            {
                ResponseExceptionEnd(context, res);
            }
        }



        private static async Task<Resp> FormatAndCheck(HttpContext context, AppIdentity appInfo, AppAuthOption appOption)
        {
            switch (appInfo.SourceMode)
            {
                // 第三方回调接口，直接放过
                case AppSourceMode.PartnerApp:
                    if (string.IsNullOrEmpty(appInfo.app_id))
                    {
                        return new Resp(SysRespTypes.AppConfigError, $"未指定PartnerName,请使用{nameof(AppPartnerMetaAttribute)}指定");                      
                    }
                    appInfo.app_client = AppClientType.Server;
                    appInfo.app_type = AppType.Outer;
                    appInfo.UDID = "WEB";
                    break;

                case AppSourceMode.AppSign:
                    string authTicketStr = context.Request.Headers[appOption.ServerSignModeHeaderName];
                    appInfo.FromTicket(authTicketStr);
                    if (!AppInfoHelper.FormatAppIdInfo(appInfo))
                    {
                        return new Resp(RespTypes.UnKnowSource, "未知应用来源！");
                    }
                    break;
                default:
                    appInfo.app_id = AppInfoHelper.AppId;
                    appInfo.app_ver = AppInfoHelper.AppVersion;
                    appInfo.app_id = AppInfoHelper.AppId;
                    appInfo.UDID = "WEB";
                    break;
            }

            var res = (await appOption?.AppProvider?.CheckApp(context, appInfo)) ?? new Resp();

            if (appInfo.SourceMode>=AppSourceMode.Browser)
            {
                if (context.Request.Cookies.TryGetValue(appOption.UserTokenCookieName,out string tokenVal))
                    appInfo.token = tokenVal;
            }
            context.CompleteAppIdentity(appInfo);
            return res;
        }
        
        private static async Task<Resp> TenantFormatAndCheck(HttpContext context, AppIdentity appInfo, AppAuthOption appOption)
        {
            if (appInfo.SourceMode == AppSourceMode.PartnerApp
                || appOption?.TenantProvider == null
                || CoreTenantContext.Identity != null)
                return new Resp();

            var identityRes = await appOption.TenantProvider.GetIdentity(context, appInfo);
            if (!identityRes.IsSuccess())
                return identityRes;

            CoreTenantContext.SetIdentity(identityRes.data);
            return identityRes;
        }


        private static AppSourceMode GetAppSourceMode(HttpContext context, AppAuthOption appOption)
        {
            if (context.Request.Headers.ContainsKey(appOption.ServerSignModeHeaderName))
            {
                return AppSourceMode.AppSign;
            }
            return AppSourceMode.Browser ;
        }
    }

    /// <summary>
    ///  应用授权参数
    /// </summary>
    public class AppAuthOption
    {
        /// <summary>
        ///   应用服务端签名模式，对应的票据信息的请求头名称
        /// </summary>
        public string ServerSignModeHeaderName { get; set; } = "at-id";

        /// <summary>
        ///  用户token 对应的cookie名称（在请求源在浏览器模式下尝试从cookie中获取用户token
        /// </summary>
        public string UserTokenCookieName { get; set; } = "u_cn";

        /// <summary>
        ///  应用授权实现
        /// </summary>
        public IAppAuthProvider AppProvider { get; set; }

        /// <summary>
        ///  租户授权实现
        /// </summary>
        public ITenantAuthProvider TenantProvider { get; set; }
    }
}
