using Microsoft.AspNetCore.Builder;
using OSS.Core.Context.Attributes.Helper;

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
        public static IApplicationBuilder UseCoreException(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CoreExceptionMiddleware>();
        }


        /// <summary>
        /// 初始化 Core 全局上下文初始化中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCoreContext(this IApplicationBuilder app, CoreContextOption option=null)
        {
            InterReqHelper.Option = option;
            return app.UseMiddleware<CoreContextMiddleware>();
        }
        
    }
}