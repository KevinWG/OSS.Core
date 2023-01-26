using OSS.Core.Context;

namespace OSS.Core.Module.Portal;

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
    /// <param name="remember">是否记住自动登录</param>
    public static void SetCookie(HttpResponse response, string token,bool remember)
    {
        if (string.IsNullOrEmpty(token))
            return;

        var appSourceMode = CoreContext.App.Identity.auth_mode;
        if (appSourceMode >= AppAuthMode.None)
        {
            var cookieOpt = new CookieOptions()
            {
                HttpOnly = true, //SameSite = SameSiteMode.None, //Secure = true,
            };
            if (remember)
            {
                cookieOpt.Expires = DateTimeOffset.Now.AddDays(30);
            }
            response.Cookies.Append(PortalConst.CookieKeys.UserCookieName, token, cookieOpt);
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
