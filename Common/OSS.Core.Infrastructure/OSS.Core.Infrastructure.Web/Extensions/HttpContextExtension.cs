using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using OSS.Core.Context.Mos;

namespace OSS.Core.Infrastructure.Web.Extensions
{
    /// <summary>
    ///  属性扩展
    /// </summary>
    public static class HttpContextExtension
    {
        ///// <summary>
        ///// 是否是Pjax请求
        ///// </summary>
        ///// <param name="req"></param>
        ///// <param name="nameSpc"></param>
        ///// <returns></returns>
        //public static bool IsPjax(this HttpRequest req, string nameSpc = null)
        //{
        //    return string.IsNullOrEmpty(nameSpc)
        //        ? req.Headers["X-PJAX"].Count > 0
        //        : req.Headers["X-PJAX"].FirstOrDefault() == nameSpc;
        //}

        /// <summary>
        ///  是否是Ajax请求
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Obsolete("无法检测到fetch请求，使用IsApiAjax-需要前端配合")]
        public static bool IsAjax(this HttpRequest req)
        {
            return IsApiAjax(req) || req.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        /// <summary>
        ///  是否是前端API类发起的ajax
        ///     -- 前端请求添加 X-App-OsVer 头信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static bool IsApiAjax(this HttpRequest req)
        {
            return req.Headers["X-App-OsVer"].Count > 0;
        }

        /// <summary>
        ///  获取IP地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetIpAddress(HttpContext context)
        {
            string ipAddress = context.Request.Headers["X-Forwarded-For"];
            return !string.IsNullOrEmpty(ipAddress) ? ipAddress : context.Connection.RemoteIpAddress.ToString();
        }

        /// <summary>
        ///  补充应用授权信息
        /// </summary>
        /// <param name="sysInfo"></param>
        /// <param name="context"></param>
        internal static void CompleteAppIdentity(this HttpContext context, AppIdentity sysInfo)
        {
            if (string.IsNullOrEmpty(sysInfo.client_ip))
                sysInfo.client_ip = GetIpAddress(context);

            if (string.IsNullOrEmpty(sysInfo.trace_num))
                sysInfo.trace_num = context.TraceIdentifier = Guid.NewGuid().ToString();
            else
                context.TraceIdentifier = sysInfo.trace_num;

            sysInfo.host = context.Request.Host.ToString();
        }
    }
}
