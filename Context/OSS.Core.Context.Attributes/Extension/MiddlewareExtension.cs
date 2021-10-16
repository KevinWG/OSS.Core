using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OSS.Core.Context.Attributes.Helper;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  异常中间件
    /// </summary>
    public static class MiddlewareExtension
    {
        /// <summary>
        /// 异常处理中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }

        /// <summary>
        /// 全局上下文初始化中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseInitialMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<InitialMiddleware>();
        }


        /// <summary>
        /// 添加OSS.Core对应的配置信息
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IServiceCollection AddCoreContextOption(this IServiceCollection services, CoreContextOption option)
        {
            InterReqHelper.Option = option;
            return services;
        }

        //
    }
}