using Microsoft.AspNetCore.Http;
using OSS.Common.Resp;
using OSS.Tools.Log;
using System.Net;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  异常处理中间件
    /// </summary>
    public class CoreExceptionMiddleware : BaseMiddleware
    {
        /// <summary>
        ///  异常处理中间件
        /// </summary>
        public CoreExceptionMiddleware(RequestDelegate next) : base(next)
        {
        }

        /// <summary>
        ///  处理方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task Invoke(HttpContext context)
        {
            Exception error;
            IResp     errorResp;

            try
            {
                await _next.Invoke(context);

                if (context.Response.StatusCode == (int) HttpStatusCode.NotFound)
                    await ExceptionResponse(context,  new Resp(RespCodes.OperateObjectNull, "当前请求资源不存在！"));

                return;
            }
            catch (RespException resEx)
            {
                errorResp = resEx.ErrorResp;
                error     = resEx;
            }
            catch (Exception ex)
            {
                errorResp = new Resp(SysRespCodes.AppError, $"当前操作失败！");
                error     = ex;
            }

            LogHelper.Error(string.Concat("请求地址:", context.Request.Path, "错误信息：", error.Message, "详细信息：", error.StackTrace),
                nameof(CoreExceptionMiddleware));

            #if DEBUG
            if (error != null)
            {
                throw error;
            }
            #endif
            
            var res = errorResp;
            await ExceptionResponse(context,  res);
        }

        /// <summary>
        ///  异常响应处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        private static Task ExceptionResponse(HttpContext context,  IResp res)
        {
            var url = InterReqHelper.GetErrorPage(context,  res);
            if (string.IsNullOrEmpty(url))
            {
                return ResponseJsonError(context.Response, res);
            }

            context.Response.Redirect(url);
            return Task.CompletedTask;
        }
    }
}
