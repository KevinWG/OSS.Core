using Microsoft.AspNetCore.Http;
using OSS.Core.Context.Attributes.Helper;

namespace OSS.Core.Context.Attributes
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
        ///  当站点使用服务端页面渲染，如果配置了   异常时会跳转
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static bool IsFetchApi(this HttpRequest req)
        {
            var headerName = InterReqHelper.Option?.JSRequestHeaderName;
            if (!string.IsNullOrEmpty(headerName)&& req.Headers.ContainsKey(headerName))
            {
                return true;
            }
            return false;
        }
    }
}
