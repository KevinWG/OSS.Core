using OSS.Core.Context;
using OSS.Core.Portal.Domain;

namespace OSS.Core.WebApis.App_Codes.AuthProviders
{
    /// <summary>
    /// 授权辅助类
    /// </summary>
    public class PortalWebHelper
    {
        /// <summary>
        ///  设置cookie
        /// </summary>
        /// <param name="response"></param>
        /// <param name="token"></param>
        public static void SetCookie(HttpResponse response, string token)
        {
            if (string.IsNullOrEmpty(token))
                return;

            var appSourceMode = CoreContext.App.Identity.source_mode;
            if (appSourceMode >= AppSourceMode.Browser)
            {
                response.Cookies.Append(PortalConst.CookieKeys.UserCookieName, token, new CookieOptions()
                {
                    HttpOnly = true, //SameSite = SameSiteMode.None, //Secure = true,
                    Expires = DateTimeOffset.Now.AddDays(3)
                });
            }
        }

        /// <summary>
        ///  清理用户Cookie
        /// </summary>
        /// <param name="response"></param>
        public static void ClearCookie(HttpResponse response)
        {
            response.Cookies.Delete(PortalConst.CookieKeys.UserCookieName);
        }
        
    }
}