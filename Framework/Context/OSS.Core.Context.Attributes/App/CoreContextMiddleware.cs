using Microsoft.AspNetCore.Http;

namespace OSS.Core.Context.Attributes
{
    /// <inheritdoc />
    public class CoreContextMiddleware : BaseMiddleware
    {
        /// <summary>
        ///  异常处理中间件
        /// </summary>
        public CoreContextMiddleware(RequestDelegate next) : base(next)
        {
        }

        /// <inheritdoc />
        public override Task Invoke(HttpContext context)
        {
            CoreContext.InitialContextContainer();

            return _next.Invoke(context);
        }
    }
}