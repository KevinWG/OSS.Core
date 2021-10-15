using System;
using Microsoft.AspNetCore.Http;
using OSS.Core.Infrastructure.Web.Helpers;

namespace OSS.Core.Infrastructure.Web
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
        [Obsolete("无法检测到fetch请求，使用IsAjaxApi(需要浏览器添加头信息)")]
        public static bool IsAjax(this HttpRequest req)
        {
            return IsFetchApi(req) || req.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        /// <summary>
        ///  是否是前端API类发起的ajax
        ///     -- 前端请求添加 x-core-app 头信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static bool IsFetchApi(this HttpRequest req)
        {
            return req.Headers.ContainsKey(AppWebInfoHelper.BrowserFetchHeaderName);
        }

    }
}
