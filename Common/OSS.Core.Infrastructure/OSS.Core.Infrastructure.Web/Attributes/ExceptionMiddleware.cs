using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Extension;
using OSS.Core.Context;
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
            AppWebInfoHelper.GetOrSetAppIdentity(context);
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
            AppSourceMode mode = AppSourceMode.BrowserWithHeader;
            try
            {
                // 需要在此初始化，否则中间件依次退出后此值为空，下方异常无法捕获APP信息
               var appInfo= AppWebInfoHelper.GetOrSetAppIdentity(context);
                mode = appInfo.SourceMode;

                await _next.Invoke(context);

                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
                    await ExceptionResponse(context, new Resp(RespTypes.ObjectNull, "当前请求资源不存在！"), mode);

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
            var res = errorResp ?? new Resp(RespTypes.InnerError, string.Concat("服务暂时不可用！详情错误码：", code));
            await ExceptionResponse(context, res, mode);
        }

        /// <summary>
        ///  异常响应处理
        /// </summary>
        /// <param name="context"></param>
        /// <param name="res"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        private static async Task ExceptionResponse(HttpContext context, Resp res,AppSourceMode mode)
        {
            if (mode == AppSourceMode.Browser
                && !AppWebInfoHelper.CheckIf404OrErrorUrl(context.Request.Path.ToString()))
            {
                var errUrl = res.IsRespType(RespTypes.ObjectNull) ? AppWebInfoHelper.NotFoundUrl : AppWebInfoHelper.ErrorUrl;
                if (!string.IsNullOrEmpty(errUrl))
                {
                    string url = string.Concat(errUrl, "?ret=", res.ret, "&msg=", errUrl.UrlEncode());
                    context.Response.Redirect(url);
                    return;
                }
            }
            await ResponseJsonError(context.Response, res);
        }
    }
    /// <summary>
    ///  异常中间件
    /// </summary>
    public static class ExceptionMiddlewareExtension
    {
        /// <summary>
        /// 异常处理中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }

        /// <summary>
        /// 全局上下文初始化中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseInitialMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<InitialMiddleware>();
        }
    }

}
