#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 全局的异常处理
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-23
*       
*****************************************************************************/

#endregion

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using OSS.Common.Plugs.LogPlug;

namespace OSS.Core.WebSite.AppCodes.Filters
{
    /// <summary>
    ///   全局的异常处理
    /// </summary>
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
