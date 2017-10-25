#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 接口全局异常处理中间件
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-20
*       
*****************************************************************************/

#endregion

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Plugs.LogPlug;

namespace OSS.Core.WebApi.Filters
{
    /// <summary>
    ///  异常处理中间件
    /// </summary>
    internal class ExceptionMiddleware : BaseMiddlewaire
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
                if (context.Response.StatusCode == (int) HttpStatusCode.NotFound)
                {
                    await ResponseEnd(context, new ResultMo(-1, "当前接口不存在！"));
                }
                return;
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#endif
                error = ex;
            }
            var code = LogUtil.Error(error.StackTrace, nameof(ExceptionMiddleware));
            await ResponseEnd(context, new ResultMo(ResultTypes.InnerError, string.Concat("出现未知错误，详细错误码：", code)));
        }

    }

    /// <summary>
    /// 异常处理中间件扩展类
    /// </summary>
    internal static class ExceptionMiddlewareExtention
    {
        internal static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }

    /// <summary>
    ///  中间件基类
    /// </summary>
    internal class BaseMiddlewaire
    {
        /// <summary>
        ///   结束请求
        /// </summary>
        /// <param name="context"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        protected static async Task ResponseEnd(HttpContext context, ResultMo res)
        {
            context.Response.Clear();
            ClearCacheHeaders(context.Response);
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsync($"{{\"ret\":{res.ret},\"message\":\"{res.msg}\"}}");
        }

        /// <summary>
        ///  清理Response缓存
        /// </summary>
        /// <param name="httpResponse"></param>
        private static void ClearCacheHeaders(HttpResponse httpResponse)
        {
            httpResponse.Headers["Cache-Control"] = "no-cache";
            httpResponse.Headers["Pragma"] = "no-cache";
            httpResponse.Headers["Expires"] = "-1";
            httpResponse.Headers.Remove("ETag");
        }
    }

}
