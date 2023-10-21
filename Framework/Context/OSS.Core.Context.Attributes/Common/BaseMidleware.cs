using Microsoft.AspNetCore.Http;

using OSS.Common.Resp;
using System.Net;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  中间件基类
    /// </summary>
    public abstract class BaseMiddleware
    {
        /// <summary>
        /// 中间件的下级执行句柄
        /// </summary>
        protected readonly RequestDelegate _next;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next"></param>
        protected BaseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        ///  执行方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract Task Invoke(HttpContext context);

        /// <summary>
        ///  清理Response缓存
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <param name="res"></param>
        protected static Task ResponseJsonError(HttpResponse httpResponse, IResp res)
        {
            httpResponse.Clear();
            httpResponse.Headers.Remove("ETag");

            httpResponse.Headers["Pragma"]        = "no-cache";
            httpResponse.Headers["Expires"]       = "-1";
            httpResponse.Headers["Cache-Control"] = "no-cache";

            httpResponse.ContentType = "application/json; charset=utf-8";
            httpResponse.StatusCode  = (int)HttpStatusCode.OK;

            return httpResponse.WriteAsync(res.ToJson());
        } 
    }



}



