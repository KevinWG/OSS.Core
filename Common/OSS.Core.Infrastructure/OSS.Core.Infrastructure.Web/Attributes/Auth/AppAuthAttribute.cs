using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Helpers;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;
using OSS.Core.Infrastructure.Web.Extensions;
using OSS.Core.Infrastructure.Web.Helpers;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth
{
    public class AppAuthAttribute: BaseOrderAuthAttribute
    {
        private readonly AppAuthOption _appOption;

        public AppAuthAttribute():this(null)
        {
        }
        public AppAuthAttribute(AppAuthOption appOption)
        {
            p_Order = -1000;
            _appOption = appOption;
            //p_IsWebSite = appOption.IsWebSite;
        }

        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // 0.  获取初始化app信息
            var appInfo = AppWebInfoHelper.GetOrSetAppIdentity(context.HttpContext);
            if (appInfo==null)
            {
                ResponseExceptionEnd(context, new Resp(SysRespTypes.AppConfigError, "请使用InitialMiddleware中间件初始化全局上下文信息"));
                return;
            }
            // 1. app验证
            var res = await AppFormatAndCheck(context.HttpContext, appInfo, _appOption);
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


        private static async Task<Resp> AppFormatAndCheck(HttpContext context, AppIdentity appInfo, AppAuthOption appOption)
        {
            Resp res;
            switch (appInfo.SourceMode)
            {
                // 第三方回调接口，直接放过
                case AppSourceMode.PartnerServer:
                    if (string.IsNullOrEmpty(appInfo.app_id))
                    {
                        res = new Resp(SysRespTypes.AppConfigError, "未指定PartnerName(请使用AppPartnerNameAttribute指定)");
                        break;
                    }
                    appInfo.app_client = AppClientType.Server;
                    appInfo.app_type   = AppType.Outer;
                    appInfo.UDID       = "WEB";

                    res = new Resp();
                    break;
                case AppSourceMode.ServerSign:
                    if (appOption?.AppProvider == null)
                    {
                        return new Resp(RespTypes.InnerError, "服务接口并未启用服务端应用校验，请求拒绝！");
                    }
                    res = await ServerAppCheck(context, appOption.AppProvider, appInfo);
                    break;
                default:
                    appInfo.app_id  = AppInfoHelper.AppId;
                    appInfo.app_ver = AppInfoHelper.AppVersion;
                    appInfo.app_id  = AppInfoHelper.AppId;
                    appInfo.UDID    = "WEB";

                    res = new Resp();
                    break;
            }
            context.CompleteAppIdentity(appInfo);          
            return res;
        }

        private static async Task<Resp> ServerAppCheck(HttpContext context, IAppAuthProvider provider, AppIdentity appInfo)
        {
            string authTicketStr = context.Request.Headers[CoreConstKeys.AppServerModeTicketName];
            appInfo.FromTicket(authTicketStr);

            var secretKeyRes = await provider.IntialAuthAppConfig(context, appInfo);
            if (!secretKeyRes.IsSuccess())
                return secretKeyRes;

            const int expireSecs = 60 * 60 * 2;
            if (!appInfo.CheckSign(secretKeyRes.data.AppSecret, expireSecs).IsSuccess()
                || !AppInfoHelper.FormatAppIdInfo(appInfo))
                return new Resp(RespTypes.SignError, "签名错误！");

            return secretKeyRes;
        }


        private static async Task<Resp> TenantFormatAndCheck(HttpContext context, AppIdentity appInfo, AppAuthOption appOption)
        {
            if (appInfo.SourceMode == AppSourceMode.PartnerServer
                || appOption?.TenantProvider == null
                || TenantContext.Identity != null)
                return new Resp();

            var identityRes = await appOption.TenantProvider.InitialAuthTenantIdentity(context, appInfo);
            if (!identityRes.IsSuccess())
                return identityRes;

            TenantContext.SetIdentity(identityRes.data);
            return identityRes;
        }

    }


    public class AppAuthOption: BaseAuthOption
    {

        public IAppAuthProvider AppProvider { get; set; }

        public ITenantAuthProvider TenantProvider { get; set; }
    }

    public abstract class BaseAuthOption
    {
        ///// <summary>
        /////  如果 IsWebSite=True 且当前请求不为ajax时，验证失败时返回json格式，否则直接重定向到 login、404、error 页面
        ///// 且 如果IsWebSite=True 无头部ticket，会通过web配置补充appid等信息
        ///// </summary>
        //public bool IsWebSite { get; set; }
    }
}
