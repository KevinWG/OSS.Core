using Microsoft.AspNetCore.Http;
using OSS.Common.Resp;
using OSS.Common.Extension;

namespace OSS.Core.Context.Attributes
{
    internal static class InterReqHelper
    {
        internal static CoreContextOption? Option { get; set; }

        internal static string GetErrorPage(HttpContext context,  IResp res)
        {
            if ((CoreContext.App.IsInitialized && CoreContext.App.Identity.auth_mode != AppAuthMode.None) || context.Request.IsFetchReq())
                return string.Empty;

            if (CheckIfErrorUrl(context.Request.Path.ToString()))
                return string.Empty;

            var errUrl = Option?.ErrorPage;
            return string.IsNullOrEmpty(errUrl)
                ? string.Empty
                : string.Concat(errUrl, "?code=", res.code, "&msg=", errUrl.SafeEscapeDataString());
        }

        internal static string GetUnLoginPage(HttpContext context)
        {
            if ((CoreContext.App.IsInitialized && CoreContext.App.Identity.auth_mode != AppAuthMode.None) || context.Request.IsFetchReq())
                return string.Empty;

            var loginUrl = Option?.LoginPage;
            if (string.IsNullOrEmpty(loginUrl))
                return string.Empty;

            var req = context.Request;
            var rUrl = string.Concat(req.Path, req.QueryString);

            return string.Concat(loginUrl, "?rurl=", rUrl.SafeEscapeDataString());
        }

        /// <summary>
        /// 检查是否已经是404页或异常页，排除防止死循环
        /// </summary>
        /// <param name="requestPath"></param>
        /// <returns></returns>
        private static bool CheckIfErrorUrl(string requestPath)
        {
            return !string.IsNullOrEmpty(Option?.ErrorPage) && requestPath.StartsWith(Option.ErrorPage);
        }

    }

}
