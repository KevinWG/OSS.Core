using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

using OSS.Common.Resp;
using OSS.Core.Context.Attributes.Common;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  应用授权过滤器
    /// </summary>
    public class AppAuthorizeAttribute: BaseOrderAuthorizeAttribute
    {
        private readonly AppAuthOption? _appOption;

        /// <summary>
        ///  构造函数
        /// </summary>
        public AppAuthorizeAttribute()
        {
            Order = AttributeConst.Order_App_AuthAttribute;
        }

        /// <summary>
        ///  构造函数
        /// </summary>
        public AppAuthorizeAttribute(AppAuthOption appOption):this()
        {
            _appOption = appOption;
        }

        /// <summary>
        ///  应用的授权判断处理
        /// </summary>
        public override async Task<IResp> Authorize(AuthorizationFilterContext context)
        {
            // 0.  获取初始化app信息
            var appIdentity = new AppIdentity();
            CoreContext.App.Identity = appIdentity;
            
            var checker = _appOption?.AppAuthProvider;
            if (checker != null)
            {
                // 1. app 内容格式化
                var res = await checker.Authorize(appIdentity, context.HttpContext);
                if (!res.IsSuccess())
                    return res;
            }

            CompleteAppIdentity(context.HttpContext, appIdentity);

            //2. Tenant 验证
            return await TenantFormatAndCheck(appIdentity, _appOption);
        }
        

        private static async Task<IResp> TenantFormatAndCheck(AppIdentity appInfo, AppAuthOption? appOption)
        {
            if (appInfo.source_mode == AppSourceMode.OutApp
                || appOption?.TenantAuthProvider == null
                || !CoreContext.Tenant.IsAuthenticated)
                return Resp.DefaultSuccess;

            var identityRes = await appOption.TenantAuthProvider.GetIdentity();
            if (!identityRes.IsSuccess())
                return identityRes;

            CoreContext.Tenant.Identity = identityRes.data;
            return identityRes;
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
    }

    /// <summary>
    ///  应用授权参数
    /// </summary>
    public class AppAuthOption
    {
        /// <summary>
        ///  应用授权实现
        /// </summary>
        public IAppAuthProvider? AppAuthProvider { get; set; }

        /// <summary>
        ///  租户授权实现
        /// </summary>
        public ITenantAuthProvider? TenantAuthProvider { get; set; }
    }
}
