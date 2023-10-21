using Microsoft.AspNetCore.Builder;
using OSS.Common.Extension;
using OSS.Tools.Config;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  异常中间件
    /// </summary>
    public static class MiddlewareExtension
    {
        /// <summary>
        /// 初始化异常处理中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseOssCoreException(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CoreExceptionMiddleware>();
        }

        /// <summary>
        /// 初始化 Core 全局应用信息 和 中间件上下文容器
        /// </summary>
        /// <param name="app"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseOssCore(this IApplicationBuilder app, CoreContextOption? option = null)
        {
            InterReqHelper.Option = option;

            CoreContext.ServiceProvider = app.ApplicationServices;
            CoreContext.App.Self        = InitialSelfAppInfo();

            return app.UseMiddleware<CoreContextMiddleware>();
        }

        private static AppInfo InitialSelfAppInfo()
        {
            var appInfo = new AppInfo
            {
                worker_id = ConfigHelper.GetSection("AppConfig:WorkerId")?.Value.ToInt32() ?? 0,
                version   = ConfigHelper.GetSection("AppConfig:Version")?.Value ?? "1.0",
                base_url  = ConfigHelper.GetSection("AppConfig:BaseUrl")?.Value ?? string.Empty
            };
            return appInfo;
        }
    }
}