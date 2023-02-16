using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

using OSS.Common.Resp;
using OSS.Core.Context.Attributes.Common;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  应用授权过滤器
    /// </summary>
    public class AppAuthorizeAttribute : BaseOrderAuthorizeAttribute
    {
        private readonly AppAuthOption _appOption;

        /// <summary>
        ///  构造函数
        /// </summary>
        public AppAuthorizeAttribute(AppAuthOption appOption)
        {
            _appOption = appOption;
            Order      = AttributeConst.Order_App_AuthAttribute;
        }

        /// <summary>
        ///  应用的授权判断处理
        /// </summary>
        public override async Task<IResp> Authorize(AuthorizationFilterContext context)
        {
            // 0.  获取初始化app信息
            if (!CoreContext.App.IsInitialized)
                CoreContext.App.Identity = new AppIdentity();
            
            var appIdentity = CoreContext.App.Identity;
            
            // 1. app 内容格式化
            var res = await AppAuthorize(appIdentity, context.HttpContext);
            if (!res.IsSuccess())
                return res;

            CompleteAppIdentity(context.HttpContext, appIdentity);

            //2. Tenant 验证
            return await TenantAuthorize(appIdentity, _appOption);
        }

        #region 应用验证


        private async Task<IResp> AppAuthorize(AppIdentity appInfo, HttpContext context)
        {
            if (appInfo.auth_mode != AppAuthMode.PartnerContract)
            {
                appInfo.auth_mode = context.Request.Headers.ContainsKey(_appOption.SignModeHeaderName)
                    ? AppAuthMode.AppSign
                    : AppAuthMode.None;
            }
            
            switch (appInfo.auth_mode)
            {
                case AppAuthMode.AppSign:
                    var res = await CheckAppSign(appInfo, context);
                    if (!res.IsSuccess())
                        return res;
                    break;

                default:
                    appInfo.app_ver    = CoreContext.App.Self.AppVersion;
                    appInfo.UDID       = "WEB";
                    break;
            }

            //  验证要求的类型
            if (appInfo.auth_mode > appInfo.ask_auth.app_auth_mode || appInfo.app_type > appInfo.ask_auth.app_type)
            {
                return new Resp(SysRespCodes.NotAllowed, "应用权限不足!");
            }


            if (appInfo.auth_mode >= AppAuthMode.None)
            {
                // 浏览器模式下，从cookie中初始化用户Token
                if (context.Request.Cookies.TryGetValue(_appOption.BrowserModeUserCookieName, out var tokenVal))
                    appInfo.token = tokenVal;
            }

            return Resp.DefaultSuccess;
        }

        private async Task<IResp> CheckAppSign(AppIdentity appIdentity, HttpContext context)
        {
            var authTicketStr = context.Request.Headers[_appOption.SignModeHeaderName];
            appIdentity.FormatFromTicket(authTicketStr);

            if (_appOption.SignAccessProvider == null)
                throw new NotImplementedException("请设置应用签名秘钥提供器(SignAccessProvider)");

            var accessRes = await _appOption.SignAccessProvider.GetByKey(appIdentity.access_key);
            if (!accessRes.IsSuccess())
                return accessRes;

            var access = accessRes.data;

            const int expireSecs = 60 * 60 * 2;
            return appIdentity.CheckSign(access.access_secret, expireSecs);
        }

        // 补充应用全局信息
        private static void CompleteAppIdentity(HttpContext context, AppIdentity sysInfo)
        {
            if (string.IsNullOrEmpty(sysInfo.client_ip))
                sysInfo.client_ip = GetIpAddress(context);

            if (string.IsNullOrEmpty(sysInfo.trace_no))
                sysInfo.trace_no = context.TraceIdentifier = Guid.NewGuid().ToString();
            else
                context.TraceIdentifier = sysInfo.trace_no;

            sysInfo.host = context.Request.Host.ToString();
        }

        // 获取IP地址
        private static string GetIpAddress(HttpContext context)
        {
            string ipAddress = context.Request.Headers["X-Forwarded-For"];
            return !string.IsNullOrEmpty(ipAddress) ? ipAddress : context.Connection.RemoteIpAddress.ToString();
        }

        #endregion


        private static async Task<IResp> TenantAuthorize(AppIdentity appInfo, AppAuthOption? appOption)
        {
            if (appOption?.TenantAuthProvider == null)
                return Resp.DefaultSuccess;

            var identityRes = await appOption.TenantAuthProvider.GetIdentity();
            if (!identityRes.IsSuccess())
                return identityRes;

            CoreContext.Tenant.Identity = identityRes.data;
            return identityRes;
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
        public string SignModeHeaderName { get; set; } = CoreContext.DefaultAppSignModeTicketHeaderName;

        /// <summary>
        ///  用户token 对应的cookie名称（在请求源在浏览器模式下尝试从cookie中获取用户token
        /// </summary>
        public string BrowserModeUserCookieName { get; set; } = CoreContext.DefaultBrowserModeUserCookieName;
        
        /// <summary>
        ///  签名秘钥提供者
        /// </summary>
        public IAppAccessProvider? SignAccessProvider { get; set; }

        /// <summary>
        ///  租户授权实现
        /// </summary>
        public ITenantAuthProvider? TenantAuthProvider { get; set; }
    }
}
