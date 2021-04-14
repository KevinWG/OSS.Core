using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Extention;
using OSS.Core.Context;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Web.Extensions;
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
        public static string NotFoundUrl { get; } = ConfigHelper.GetSection("AppWebConfig:404")?.Value;

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

        internal static AppIdentity GetOrSetAppIdentity(HttpContext context)
        {
            var sysInfo = AppReqContext.Identity;
            if (sysInfo != null) return sysInfo;

            sysInfo = new AppIdentity {SourceMode = GetAppSourceMode(context)};

            AppReqContext.SetIdentity(sysInfo);
            return sysInfo;
        }



        /// <summary>
        /// 检查是否已经是404页或异常页，排除防止死循环
        /// </summary>
        /// <param name="requestPath"></param>
        /// <returns></returns>
        public static bool CheckIf404OrErrorUrl(string requestPath)
        {
            var isUnUrl = (!string.IsNullOrEmpty(NotFoundUrl) && requestPath.StartsWith(NotFoundUrl))
                          || (!string.IsNullOrEmpty(ErrorUrl) && requestPath.StartsWith(ErrorUrl));

            return isUnUrl;
        }

        /// <summary>
        /// 获取应用的来源模式
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static AppSourceMode GetAppSourceMode(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(CoreConstKeys.AppServerModeTicketName))
            {
                return AppSourceMode.ServerSign;
            }

            var path = context.Request.Path.ToString();
            if (path.StartsWith("/partner/"))
                return AppSourceMode.PartnerServer;

            return context.Request.IsAjaxApi() ?
               AppSourceMode.BrowserWithHeader :
               AppSourceMode.Browser;
        }

        #endregion

        private static string GetReqReferer(HttpRequest req)
        {
            return req.Headers[HeaderNames.Referer].FirstOrDefault() ?? "/";
        }
    }

}
