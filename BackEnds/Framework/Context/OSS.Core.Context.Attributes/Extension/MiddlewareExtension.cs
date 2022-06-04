using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OSS.Common.Extension;
using OSS.Core.Context.Attributes.Helper;
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
        public static IApplicationBuilder UseOssCore(this IApplicationBuilder app, CoreContextOption option=null)
        {
            InterReqHelper.Option = option;

            ConfigHelper.Configuration = app.ApplicationServices.GetService<IConfiguration>();

            CoreContext.ServiceProvider = app.ApplicationServices;

            CoreContext.App.Self = InitialSelfAppInfo();

            return app.UseMiddleware<CoreContextMiddleware>();
        }

        private static AppInfo InitialSelfAppInfo() {

            var appInfo = new AppInfo
            {
                AppId = ConfigHelper.GetSection("AppConfig:AppId")?.Value,
                AppSecret = ConfigHelper.GetSection("AppConfig:AppSecret")?.Value,

                AppWorkerId = ConfigHelper.GetSection("AppConfig:AppWorkerId")?.Value.ToInt32() ?? 0,
                AppVersion = ConfigHelper.GetSection("AppConfig:AppVersion")?.Value
            };

            return appInfo;
        }
    }
}