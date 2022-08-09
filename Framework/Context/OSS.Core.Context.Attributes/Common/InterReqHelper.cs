using Microsoft.AspNetCore.Http;
using OSS.Common.Resp;
using OSS.Common.Extension;

namespace OSS.Core.Context.Attributes
{
    internal static class InterReqHelper
    {
        internal static CoreContextOption Option { get; set; }

        internal static string GetErrorPage(HttpContext context, AppIdentity appInfo, IResp res)
        {
            if (appInfo.source_mode != AppSourceMode.Browser || context.Request.IsFetchApi())
                return string.Empty;

            if (CheckIfErrorUrl(context.Request.Path.ToString()))
                return string.Empty;

            var errUrl = Option?.ErrorPage;
            return string.IsNullOrEmpty(errUrl)
                ? string.Empty
                : string.Concat(errUrl, "?code=", res.code, "&msg=", errUrl.SafeEscapeUriDataString());
        }

        internal static string GetUnloginPage(HttpContext context, AppIdentity appInfo)
        {
            if (appInfo.source_mode != AppSourceMode.Browser || context.Request.IsFetchApi())
                return string.Empty;

            var loginUrl = Option?.LoginPage;
            if (string.IsNullOrEmpty(loginUrl))
                return string.Empty;

            var req = context.Request;
            var rUrl = string.Concat(req.Path, req.QueryString);

            return string.Concat(loginUrl, "?rurl=", rUrl.SafeEscapeUriDataString());
        }

        /// <summary>
        /// 检查是否已经是404页或异常页，排除防止死循环
        /// </summary>
        /// <param name="requestPath"></param>
        /// <returns></returns>
        private static bool CheckIfErrorUrl(string requestPath)
        {
            return !string.IsNullOrEmpty(Option.ErrorPage) && requestPath.StartsWith(Option.ErrorPage);
        }

    }

}
