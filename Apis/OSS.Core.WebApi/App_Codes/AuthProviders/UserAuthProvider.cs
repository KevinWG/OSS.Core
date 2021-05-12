using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Helpers;
using OSS.Core.Context;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Services.Basic.Portal.IProxies;

namespace OSS.Core.CoreApi.App_Codes.AuthProviders
{
    public class UserAuthProvider : IUserAuthProvider
    {
        public Task<Resp<UserIdentity>> InitialIdentity(HttpContext context, AppIdentity appinfo)
        {
            return PortalAuthHelper.GetMyself(context.Request);
        }       
    }

    public class PortalAuthHelper
    {

        public static Task<Resp<UserIdentity>> GetMyself(HttpRequest req)
        {
            var appinfo = AppReqContext.Identity;
            if (appinfo.SourceMode >= AppSourceMode.BrowserWithHeader)
            {
                appinfo.token = GetCookie(req);
            }
            return InsContainer<IPortalServiceProxy>.Instance.GetMyself();
        }

        public static void SetCookie(HttpResponse response, string token)
        {
            response.Cookies.Append(CookieKeys.UserCookieName, token,
                new CookieOptions() {
                    HttpOnly = true, 
                    //SameSite = SameSiteMode.None, 
                    //Secure = true,
                    Expires = DateTimeOffset.Now.AddDays(30) 
                });
        }

        public static string GetCookie(HttpRequest req)
        {
            return req.Cookies[CookieKeys.UserCookieName];
        }

        public static void ClearCookie(HttpResponse response)
        {
            response.Cookies.Delete(CookieKeys.UserCookieName);
        }
    }
}
