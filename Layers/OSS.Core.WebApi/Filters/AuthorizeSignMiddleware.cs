#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 签名验证中间件
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Core.Infrastructure.Utils;

namespace OSS.Core.WebApi.Filters
{
    /// <summary>
    ///  签名验证中间件
    /// </summary>
    internal class AuthorizeSignMiddleware: BaseMiddlewaire
    {
        private readonly RequestDelegate _next;

        public AuthorizeSignMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task Invoke(HttpContext context)
        {
            string auticketStr = context.Request.Headers[GlobalKeysUtil.AuthorizeTicketName];
            if (string.IsNullOrEmpty(auticketStr))
            {
                await ResponseEnd(context, new ResultMo(ResultTypes.UnKnowSource, "未知应用来源"));
                return;
            }

            var sysInfo = new SysAuthorizeInfo();
            sysInfo.FromSignData(auticketStr);

            var secretKeyRes = ApiSourceKeyUtil.GetAppSecretKey(sysInfo.AppSource);
            if (!secretKeyRes.IsSuccess())
            {
                await ResponseEnd(context, secretKeyRes);
                return;
            }

            if (!sysInfo.CheckSign(secretKeyRes.data))
            {
                await ResponseEnd(context, new ResultMo(ResultTypes.ParaError, "非法应用签名！"));
                return;
            }

            CompleteAuthInfo(sysInfo, context);
            MemberShiper.SetAppAuthrizeInfo(sysInfo);

            await _next.Invoke(context);
        }

 

        /// <summary>
        ///   完善授权信息
        /// </summary>
        /// <param name="sysInfo"></param>
        /// <param name="context"></param>
        private static void CompleteAuthInfo(SysAuthorizeInfo sysInfo,HttpContext context)
        {
            if (string.IsNullOrEmpty(sysInfo.IpAddress))
                sysInfo.IpAddress = GetIpAddress(context);
            //  todo  applcient , webbrowser
        }

        /// <summary>
        ///  获取IP地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetIpAddress(HttpContext context)
        {
            string ipAddress = context.Request.Headers["X-Forwarded-For"];
            return !string.IsNullOrEmpty(ipAddress) ? ipAddress : context.Connection.RemoteIpAddress.ToString();
        }
    }


    internal static class AuthorizeSignMiddlewareExtention
    {
        internal static IApplicationBuilder UseAuthorizeSignMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AuthorizeSignMiddleware>();
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
            ClearCacheHeaders(context.Response);
            context.Response.StatusCode = (int) HttpStatusCode.OK;
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync($"{{\"ret\":{res.ret},\"message\":\"{res.message}\"}}");
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
