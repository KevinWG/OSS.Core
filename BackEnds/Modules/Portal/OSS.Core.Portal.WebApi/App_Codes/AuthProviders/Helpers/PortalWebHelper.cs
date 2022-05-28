//using Microsoft.AspNetCore.Http;
//using OSS.Core.Context;
//using System;
//using OSS.Core.Common.Const;

//namespace OSS.Core.WebApis.App_Codes.AuthProviders
//{
//    /// <summary>
//    /// 授权辅助类
//    /// </summary>
//    public class PortalWebHelper
//    {
//        /// <summary>
//        ///  设置cookie
//        /// </summary>
//        /// <param name="response"></param>
//        /// <param name="token"></param>
//        public static void SetCookie(HttpResponse response, string token)
//        {
//            if (string.IsNullOrEmpty(token))
//                return;

//            var appSourceMode = CoreContext.App.Identity.source_mode;
//            if (appSourceMode >= AppSourceMode.Browser)
//            {
//                response.Cookies.Append(CookieKeys.UserCookieName, token, new CookieOptions()
//                {
//                    HttpOnly = true, //SameSite = SameSiteMode.None, //Secure = true,
//                    Expires = DateTimeOffset.Now.AddDays(3)
//                });
//            }
//        }

//        /// <summary>
//        ///  清理用户Cookie
//        /// </summary>
//        /// <param name="response"></param>
//        public static void ClearCookie(HttpResponse response)
//        {
//            response.Cookies.Delete(CookieKeys.UserCookieName);
//        }

//        ///// <summary>
//        /////  获取登录信息
//        ///// </summary>
//        ///// <param name="req"></param>
//        ///// <returns></returns>
//        //public static Task<Resp<UserIdentity>> GetIdentity(HttpRequest req)
//        //{
//        //    var appinfo = CoreContext.App.Identity;
//        //    if (appinfo.SourceMode >= AppSourceMode.BrowserWithHeader)
//        //    {
//        //        appinfo.token = GetCookie(req);
//        //    }
//        //    return InsContainer<IPortalServiceProxy>.Instance.GetIdentity();
//        //}

//        ///// <summary>
//        /////  获取用户cookie
//        ///// </summary>
//        ///// <param name="req"></param>
//        ///// <returns></returns>
//        //public static string GetCookie(HttpRequest req)
//        //{
//        //    return req.Cookies[CookieKeys.UserCookieName];
//        //}

//    }
//}