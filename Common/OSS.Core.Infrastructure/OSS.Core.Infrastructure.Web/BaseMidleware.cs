using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Web.Extensions;
using OSS.Core.Infrastructure.Web.Helpers;

namespace OSS.Core.Infrastructure.Web
{
    public abstract class BaseMiddleware
    {
        protected bool p_IsWebSite = false;
        protected readonly RequestDelegate _next;

        protected BaseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        protected async Task ResponseEnd( HttpContext context, Resp res)
        {
            if (!p_IsWebSite)
            {
                await ClearCacheHeaders(context.Response, res);
                return;
            }

            if (context.Request.IsApiAjax())
            {
                res.msg = AppWebInfoHelper.GetRedirectUrl(context, res, true);
                await ClearCacheHeaders(context.Response, res);
                return;
            }

            if (AppWebInfoHelper.CheckWebUnRedirectUrl(context.Request.Path.ToString()))
            {
                if (_next != null)
                    await _next.Invoke(context);

                return;
            }

            var redirectUrl = AppWebInfoHelper.GetRedirectUrl(context, res, false);
            context.Response.Redirect(redirectUrl);
        }

        /// <summary>
        ///  清理Response缓存
        /// </summary>
        /// <param name="httpResponse"></param>
        private static Task ClearCacheHeaders(HttpResponse httpResponse,Resp res)
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
