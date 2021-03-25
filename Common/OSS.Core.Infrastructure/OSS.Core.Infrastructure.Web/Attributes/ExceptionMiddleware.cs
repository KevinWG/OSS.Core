using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Web.Helpers;
using OSS.Tools.Log;

namespace OSS.Core.Infrastructure.Web.Attributes
{
    /// <summary>
    ///  异常处理中间件
    /// </summary>
    public class ExceptionMiddleware : BaseMiddleware
    {
        /// <summary>
        ///  异常处理中间件
        /// </summary>
        public ExceptionMiddleware(RequestDelegate next):base(next)
        {
        }

        /// <summary>
        ///  处理方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            Exception error;
            try
            {
                // 需要在此初始化，否则中间件依次退出后此值为空，下方异常无法捕获APP信息
                AppWebInfoHelper.InitialDefaultAppIdentity(context);

                await _next.Invoke(context);

                if (context.Response.StatusCode == (int) HttpStatusCode.NotFound)
                    await ResponseEnd(context, new Resp(RespTypes.ObjectNull, "当前请求资源不存在！"));

                return;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            var code = LogHelper.Error(string.Concat("请求地址:",context.Request.Path,"错误信息：", error.Message, "详细信息：", error.StackTrace),
                nameof(ExceptionMiddleware));

#if DEBUG
            if (error != null)
            {
                throw error;
            }
#endif
            await ResponseEnd(context, new Resp(RespTypes.InnerError, string.Concat("服务暂时不可用！详情错误码：", code)));
        }
    }

    public static class ExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app,bool isWebSite=false)
        {
            if (isWebSite)
            {
                return app.UseMiddleware<WebExceptionMiddleware>();
            }
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
  
}
