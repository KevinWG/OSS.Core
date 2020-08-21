using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Extention;
using OSS.Core.Context;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Helpers;
using OSS.Tools.Config;

namespace OSS.Core.Infrastructure.Web.Helpers
{
    public static class AppWebInfoHelper 
    {
        /// <summary>
        ///  脚本和CSS静态文件域名地址
        /// </summary>
        public static string CssDomain { get; } = ConfigHelper.GetSection("AppWebConfig:CssDomain")?.Value;

        /// <summary>
        ///  登录页
        /// </summary>
        public static string LoginUrl { get; } = ConfigHelper.GetSection("AppWebConfig:LoginUrl")?.Value;

        /// <summary>
        ///  404页
        /// </summary>
        public static string NotFoundUrl { get; } = ConfigHelper.GetSection("AppWebConfig:NotFoundUrl")?.Value;

        /// <summary>
        ///  错误页
        /// </summary>
        public static string ErrorUrl { get; } = ConfigHelper.GetSection("AppWebConfig:ErrorUrl")?.Value;

        ///// <summary>
        /////  获取页面缓存ETag（追加当前租户信息修改时间，防止租户信息修改后前端没有变化）
        ///// </summary>
        ///// <returns></returns>
        //public static string GetPjaxTagWithTenant()
        //{
        //    var tenant = TenantContext.Identity;
        //    return string.Concat("osspage",AppInfoHelper.AppVersion?.GetHashCode(), tenant?.last_update_time, tenant?.id);
        //}

        #region 中间件相关地址判断

        internal static AppIdentity InitialDefaultAppIdentity(HttpContext context)
        {
            var sysInfo = AppReqContext.Identity;
            if (sysInfo != null) return sysInfo;

            sysInfo = new AppIdentity {is_partner = CheckIfPartnerCall(context)};

            AppReqContext.SetIdentity(sysInfo);
            return sysInfo;
        }

        //检查是否已经是404页或异常页，排除防止死循环
        internal static bool CheckWebUnRedirectUrl(string requestPath)
        {
            var isUnUrl = (!string.IsNullOrEmpty(NotFoundUrl) && requestPath.StartsWith(NotFoundUrl))
                          || (!string.IsNullOrEmpty(ErrorUrl) && requestPath.StartsWith(ErrorUrl));

            return isUnUrl;
        }

        internal static string GetRedirectUrl(HttpContext context, Resp res, bool isAjax)
        {
            var req = context.Request;
            if (res.IsRespType(RespTypes.UnLogin))
            {
                var rUrl = isAjax ? GetReqReferer(req) : string.Concat(req.Path, req.QueryString);
                return string.Concat(LoginUrl, "?rurl=" + rUrl.UrlEncode());
            }

            if (isAjax)
                return res.msg;

            var errUrl = res.IsRespType(RespTypes.ObjectNull) ? NotFoundUrl : ErrorUrl;
            return string.Concat(errUrl, "?ret=", res.ret, "&msg=", res.msg.UrlEncode());
        }

        internal static bool CheckIfPartnerCall(HttpContext context)
        {
            var path = context.Request.Path.ToString();
            return path.StartsWith("/partner/");
        }

        #endregion

        private static string GetReqReferer(HttpRequest req)
        {
            return req.Headers[HeaderNames.Referer].FirstOrDefault() ?? "/";
        }
    }

}
