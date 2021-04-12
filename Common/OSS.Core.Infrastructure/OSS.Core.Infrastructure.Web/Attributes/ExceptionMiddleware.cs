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
    public class InitialMiddleware : BaseMiddleware
    {
        /// <summary>
        ///  异常处理中间件
        /// </summary>
        public InitialMiddleware(RequestDelegate next) : base(next)
        {
        }

        public override Task Invoke(HttpContext context)
        {
            // 防止忘记，独立初始化处理
            AppWebInfoHelper.InitialDefaultAppIdentity(context);
            return _next.Invoke(context);

        }
    }

    /// <summary>
    ///  异常处理中间件
    /// </summary>
    public class ExceptionMiddleware : BaseMiddleware
    {
        /// <summary>
        ///  异常处理中间件
        /// </summary>
        public ExceptionMiddleware(RequestDelegate next) : base(next)
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
            Resp errorResp = null;
            try
            {
                // 需要在此初始化，否则中间件依次退出后此值为空，下方异常无法捕获APP信息
                AppWebInfoHelper.InitialDefaultAppIdentity(context);

                await _next.Invoke(context);

                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
                    await ExceptionResponse(context, new Resp(RespTypes.ObjectNull, "当前请求资源不存在！"));

                return;
            }
            catch (RespException resEx)
            {
                errorResp = new Resp().WithException(resEx);
                error = resEx;
            }
            catch (Exception ex)
            {
                error = ex;
            }

            var code = LogHelper.Error(string.Concat("请求地址:", context.Request.Path, "错误信息：", error.Message, "详细信息：", error.StackTrace),
                nameof(ExceptionMiddleware));

#if DEBUG
            if (error != null)
            {
                throw error;
            }
#endif

            await ExceptionResponse(context, errorResp ?? new Resp(RespTypes.InnerError, string.Concat("服务暂时不可用！详情错误码：", code)));
        }
    }

    public static class ExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }

        public static IApplicationBuilder UseInitialMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<InitialMiddleware>();
        }
    }

}
