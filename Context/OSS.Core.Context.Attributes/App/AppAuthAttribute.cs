using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context.Attributes.Helper;

namespace OSS.Core.Context.Attributes
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
        public AppAuthAttribute()
        {
            p_Order = -1000;
        }

        /// <summary>
        ///  构造函数
        /// </summary>
        public AppAuthAttribute(AppAuthOption appOption):this()
        {
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
            var appInfo = context.HttpContext.InitialContextAppIdentity();
         
            // 1. app 内容格式化
            var res = (await _appOption?.AppProvider?.AppAuthorize(appInfo, context.HttpContext)) ?? InterReqHelper.SuccessResp;
            if (!res.IsSuccess())
                ResponseExceptionEnd(context, appInfo, res);
            
            CompleteAppIdentity(context.HttpContext,appInfo);

            //2. Tenant 验证
            res = await TenantFormatAndCheck(context.HttpContext, appInfo, _appOption);
            if (!res.IsSuccess())
            {
                ResponseExceptionEnd(context, appInfo, res);
            }
        }

        
     

        private static async Task<Resp> TenantFormatAndCheck(HttpContext context, AppIdentity appInfo, AppAuthOption appOption)
        {
            if (appInfo.source_mode == AppSourceMode.OutApp
                || appOption?.TenantProvider == null
                || CoreTenantContext.Identity != null)
                return InterReqHelper.SuccessResp;

            var identityRes = await appOption.TenantProvider.GetIdentity(context, appInfo);
            if (!identityRes.IsSuccess())
                return identityRes;

            CoreTenantContext.SetIdentity(identityRes.data);
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
        public IAppAuthProvider AppProvider { get; set; }

        /// <summary>
        ///  租户授权实现
        /// </summary>
        public ITenantAuthProvider TenantProvider { get; set; }
    }
}
