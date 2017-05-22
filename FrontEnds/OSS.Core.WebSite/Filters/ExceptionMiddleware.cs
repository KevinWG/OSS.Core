using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using OSS.Common.Plugs.LogPlug;

namespace OSS.Core.WebSite.Filters
{
    internal class ExceptionMiddleware 
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Exception error;
            try
            {
                await _next.Invoke(context);

                if (context.Response.StatusCode==(int)HttpStatusCode.NotFound)
                {
                    context.Response.Redirect("/unnormal/notfound");
                }
                return;
            }
            catch (Exception ex)
            {
                error = ex;
            }
            var code = LogUtil.Error(error.StackTrace, nameof(ExceptionMiddleware));
            context.Response.Redirect(string.Concat("/unnormal/error?code=", code));
        }
    }

    internal static class ExceptionMiddlewareExtention
    {
        internal static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
