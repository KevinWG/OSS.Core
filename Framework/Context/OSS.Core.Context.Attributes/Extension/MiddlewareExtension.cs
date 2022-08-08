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
        /// 初始化 Core 全局上下文初始化中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseOssCore(this IApplicationBuilder app, CoreContextOption option = null)
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
                AppWorkerId = ConfigHelper.GetSection("AppConfig:AppWorkerId")?.Value.ToInt32() ?? 0,
                AppVersion  = ConfigHelper.GetSection("AppConfig:AppVersion")?.Value ?? string.Empty,

                AccessKey    = ConfigHelper.GetSection("AppConfig:AccessKey")?.Value ?? string.Empty,
                AccessSecret = ConfigHelper.GetSection("AppConfig:AccessSecret")?.Value ?? string.Empty
            };

            return appInfo;
        }
    }
}