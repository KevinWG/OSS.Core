using Microsoft.AspNetCore.Http;
using OSS.Common.Resp;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  应用上下文的扩展方法
    /// </summary>
    internal static class AppIdentityExtension
    {
        /// <summary>
        ///  初始化当前请求上下文的应用全局信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static AppIdentity GetAppIdentity(this HttpContext context)
        {
            var sysInfo = CoreContext.App.Identity;
            if (sysInfo == null)
                throw new RespException(SysRespTypes.AppError,$"请先设置({nameof(CoreContextMiddleware)})中间件");

            return sysInfo;
        }


        /// <summary>
        ///  初始化当前请求上下文的应用全局信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static AppIdentity InitialCoreAppIdentity(this HttpContext context)
        {
            var sysInfo = CoreContext.App.Identity;
            if (sysInfo != null)
                return sysInfo;

            sysInfo = new AppIdentity();

            CoreContext.App.Identity = sysInfo;
            return sysInfo;
        }
    }
}
