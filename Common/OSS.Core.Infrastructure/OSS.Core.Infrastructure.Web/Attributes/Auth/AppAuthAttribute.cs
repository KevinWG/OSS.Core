using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Helpers;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;
using OSS.Core.Infrastructure.Web.Extensions;
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
            var appInfo = AppReqContext.Identity;
            if (appInfo==null)
            {
                ResponseExceptionEnd(context, new Resp(SysRespTypes.AppConfigError, "请使用InitialMiddleware中间件初始化全局上下文信息"));
                return;
            }
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
                case AppSourceMode.PartnerServer:
                    if (string.IsNullOrEmpty(appInfo.app_id))
                    {
                        return new Resp(SysRespTypes.AppConfigError, "未指定PartnerName(请使用AppPartnerNameAttribute指定)");                      
                    }
                    appInfo.app_client = AppClientType.Server;
                    appInfo.app_type = AppType.Outer;
                    appInfo.UDID = "WEB";
                    break;

                case AppSourceMode.ServerSign:
                    string authTicketStr = context.Request.Headers[AppWebInfoHelper.ServerSignModeHeaderName];
                    appInfo.FromTicket(authTicketStr);
                    if (!AppInfoHelper.FormatAppIdInfo(appInfo))
                    {
                        return new Resp(RespTypes.UnKnowSource, "未知应用来源！");
                    }
                    //if (appOption?.AppProvider == null)
                    //{
                    //    return new Resp(RespTypes.InnerError, "服务接口并未启用服务端应用校验，请求拒绝！");
                    //}
                    //res = await ServerAppCheck(context, appOption.AppProvider, appInfo);
                    break;
                default:
                    appInfo.app_id = AppInfoHelper.AppId;
                    appInfo.app_ver = AppInfoHelper.AppVersion;
                    appInfo.app_id = AppInfoHelper.AppId;
                    appInfo.UDID = "WEB";
                    break;
            }

            var res = (await appOption?.AppProvider?.AppAuthCheck(context, appInfo)) ?? new Resp();

            context.CompleteAppIdentity(appInfo);
            return res;
        }

        //private static async Task<Resp> ServerAppCheck(HttpContext context, IAppAuthProvider provider, AppIdentity appInfo)
        //{
        //    var secretKeyRes = await provider.IntialAuthAppConfig(context, appInfo);
        //    if (!secretKeyRes.IsSuccess())
        //        return secretKeyRes;

        //    const int expireSecs = 60 * 60 * 2;
        //    if (!appInfo.CheckSign(secretKeyRes.data.AppSecret, expireSecs).IsSuccess()
        //        || !AppInfoHelper.FormatAppIdInfo(appInfo))
        //        return new Resp(RespTypes.SignError, "签名错误！");

        //    return secretKeyRes;
        //}


        private static async Task<Resp> TenantFormatAndCheck(HttpContext context, AppIdentity appInfo, AppAuthOption appOption)
        {
            if (appInfo.SourceMode == AppSourceMode.PartnerServer
                || appOption?.TenantProvider == null
                || TenantContext.Identity != null)
                return new Resp();

            var identityRes = await appOption.TenantProvider.CheckAndInitialIdentity(context, appInfo);
            if (!identityRes.IsSuccess())
                return identityRes;

            TenantContext.SetIdentity(identityRes.data);
            return identityRes;
        }

    }

    /// <summary>
    ///  应用授权参数
    /// </summary>
    public class AppAuthOption
    {
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
