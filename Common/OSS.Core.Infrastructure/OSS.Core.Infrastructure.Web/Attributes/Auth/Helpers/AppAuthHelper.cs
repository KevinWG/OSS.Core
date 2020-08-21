using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Helpers;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;
using OSS.Core.Infrastructure.Web.Extensions;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth.Helpers
{
    public static class AppAuthHelper
    {

        public static async Task<Resp> FormatAndCheck(HttpContext context, AppIdentity appInfo, AppAuthOption appOption)
        {
            // 第三方回调接口，直接放过
            if (appInfo.is_partner)
            {
                appInfo.is_partner = true;
                appInfo.app_client = AppClientType.Server;
                appInfo.app_type   = AppType.Outer;
                appInfo.UDID       = "WEB";

                return new Resp();
            }
            
            Resp res;
            if (appOption.IsWebSite)
            {
                appInfo.app_ver = AppInfoHelper.AppVersion;
                appInfo.app_id  = AppInfoHelper.AppId;
                appInfo.UDID    = "WEB";

                appInfo.token = context.Request.Cookies[CookieKeys.UserCookieName];
                res = new Resp();
            }
            else
            {
                string authTicketStr = context.Request.Headers[CookieKeys.AuthorizeTicketName];
                appInfo.FromTicket(authTicketStr);
                res = await CheckAppAuthIdentity(context, appOption.AppProvider, appInfo);
            }

            context.CompleteAppIdentity(appInfo);
            return res;
        }



        private static async Task<Resp> CheckAppAuthIdentity(HttpContext context, IAppAuthProvider provider, AppIdentity appInfo)
        {
            var secretKeyRes = await provider.IntialAuthAppConfig(context, appInfo);
            if (!secretKeyRes.IsSuccess())
                return secretKeyRes;

            const int expireSecs = 60 * 60 * 2;
            if (!appInfo.CheckSign(secretKeyRes.data.AppSecret, expireSecs).IsSuccess()
                || !AppInfoHelper.FormatAppIdInfo(appInfo))
                return new Resp(RespTypes.SignError, "签名错误！");

            return secretKeyRes;
        }

    }



}
