using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Extension;

namespace OSS.Core.Context.Attributes.Helper
{
    internal static class InterReqHelper
    {
        internal static readonly Resp SuccessResp = new Resp();

        internal static CoreContextOption Option { get; set; }

        internal static string GetNotFoundOrErrorPage(HttpContext context, AppIdentity appInfo, Resp res)
        {
            if (appInfo.source_mode != AppSourceMode.Browser || context.Request.IsFetchApi())
                return string.Empty;

            if (CheckIf404OrErrorUrl(context.Request.Path.ToString()))
                return string.Empty;

            var errUrl = res.IsRespType(RespTypes.OperateObjectNull) ? Option.NotFoundPage : Option.ErrorPage;
            return string.IsNullOrEmpty(errUrl)
                ? string.Empty
                : string.Concat(errUrl, "?ret=", res.ret, "&msg=", errUrl.SafeEscapeUriDataString());
        }

        internal static string GetNotUnloginPage(HttpContext context, AppIdentity appInfo, Resp res,string loginUrl)
        {
            if (appInfo.source_mode != AppSourceMode.Browser || context.Request.IsFetchApi())
                return string.Empty;

            if (string.IsNullOrEmpty(loginUrl)) 
                return string.Empty;

            var req  = context.Request;
            var rUrl = string.Concat(req.Path, req.QueryString);

            return string.Concat(loginUrl, "?rurl=", rUrl.SafeEscapeUriDataString());

        }

        /// <summary>
        /// 检查是否已经是404页或异常页，排除防止死循环
        /// </summary>
        /// <param name="requestPath"></param>
        /// <returns></returns>
        private static bool CheckIf404OrErrorUrl(string requestPath)
        {
            var isUnUrl = (!string.IsNullOrEmpty(Option.NotFoundPage) && requestPath.StartsWith(Option.NotFoundPage))
                          || (!string.IsNullOrEmpty(Option.ErrorPage) && requestPath.StartsWith(Option.ErrorPage));

            return isUnUrl;
        }

      
    }

}
