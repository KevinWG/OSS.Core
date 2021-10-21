using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context.Attributes.Helper;
using OSS.Tools.Log;

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
            Exception     error     = null;
            IReadonlyResp errorResp = null;

            // 需要在此初始化，否则中间件依次退出后此值为空，下方异常无法捕获APP信息
            var appInfo = context.InitialCoreAppIdentity();
            try
            {
                await _next.Invoke(context);

                if (context.Response.StatusCode == (int) HttpStatusCode.NotFound)
                    await ExceptionResponse(context, appInfo, new Resp(RespTypes.OperateObjectNull, "当前请求资源不存在！"));

                return;
            }
            catch (RespException resEx)
            {
                errorResp = resEx;
                error     = resEx;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            var code = LogHelper.Error(string.Concat("请求地址:", context.Request.Path, "错误信息：", error.Message, "详细信息：", error.StackTrace),
                nameof(CoreExceptionMiddleware));
#if DEBUG
            if (error != null)
            {
                throw error;
            }
#endif
            var res = errorResp ?? new Resp(SysRespTypes.AppError, $"当前服务异常（{ error.Message}）！");
            await ExceptionResponse(context, appInfo, res);
        }

        /// <summary>
        ///  异常响应处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="appInfo"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        private static Task ExceptionResponse(HttpContext context, AppIdentity appInfo, IReadonlyResp res)
        {
            var url = InterReqHelper.GetNotFoundOrErrorPage(context, appInfo, res);
            if (string.IsNullOrEmpty(url))
            {
                return ResponseJsonError(context.Response, res);
            }
            context.Response.Redirect(url);
            return Task.CompletedTask;
        }
    }
}
