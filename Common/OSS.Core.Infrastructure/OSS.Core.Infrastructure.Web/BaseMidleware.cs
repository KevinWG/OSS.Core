using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;

namespace OSS.Core.Infrastructure.Web
{
    public abstract class BaseMiddleware
    {
        protected readonly RequestDelegate _next;

        protected BaseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        protected virtual  Task ExceptionResponse(HttpContext context, Resp res)
        {
            return ResponseJsonError(context.Response, res);
        }

        public abstract Task Invoke(HttpContext context);
        //protected virtual async Task ExceptionResponse(HttpContext context, Resp res)
        //{
        //    var appSourceMode = AppWebInfoHelper.GetAppSourceMode(context);
        //    if (appSourceMode < AppSourceMode.Browser)
        //    {
        //        await ResponseJsonError(context.Response, res);
        //        return;
        //    }

        //    if (context.Request.IsAjaxApi())
        //    {
        //        res.msg = AppWebInfoHelper.GetRedirectUrl(context, res, true);
        //        await ResponseJsonError(context.Response, res);
        //        return;
        //    }

        //    if (AppWebInfoHelper.CheckWebUnRedirectUrl(context.Request.Path.ToString()))
        //    {
        //        if (_next != null)
        //            await _next.Invoke(context);

        //        return;
        //    }

        //    var redirectUrl = AppWebInfoHelper.GetRedirectUrl(context, res, false);
        //    context.Response.Redirect(redirectUrl);
        //}

        /// <summary>
        ///  清理Response缓存
        /// </summary>
        /// <param name="httpResponse"></param>
        private static Task ResponseJsonError(HttpResponse httpResponse,Resp res)
        {
            httpResponse.Clear();
            httpResponse.Headers.Remove("ETag");

            httpResponse.Headers["Pragma"]        = "no-cache";
            httpResponse.Headers["Expires"]       = "-1";
            httpResponse.Headers["Cache-Control"] = "no-cache";

            httpResponse.ContentType = "application/json; charset=utf-8";
            httpResponse.StatusCode  = (int)HttpStatusCode.OK;
            return httpResponse.WriteAsync($"{{\"ret\":{res.ret},\"msg\":\"{res.msg}\"}}");
        } 
    }


}
