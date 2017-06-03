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
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Plugs.LogPlug;

namespace OSS.Core.WebSite.AppCodes.Filters
{
    /// <summary>
    ///   全局的异常处理
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
                    await ResponseEnd(context, new ResultMo(ResultTypes.ObjectNull, "当前请求资源不存在！"));

                return;
            }
            catch (Exception ex)
            {
                error = ex;
            }
            LogUtil.Error(string.Concat("错误信息：", error.Message, "详细信息：", error.StackTrace), nameof(ExceptionMiddleware));
            await ResponseEnd(context, new ResultMo(ResultTypes.InnerError, "服务暂时不可用！"));
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
