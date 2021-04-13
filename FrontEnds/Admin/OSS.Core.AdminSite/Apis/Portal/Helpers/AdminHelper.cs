using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Helpers;
using OSS.CorePro.AdminSite.AppCodes;
using OSS.CorePro.TAdminSite.Apis.Permit.Helpers;
using OSS.Tools.Cache;

namespace OSS.CorePro.TAdminSite.Apis.Portal.Helpers
{
    public static class AdminHelper
    {
        internal static async Task<Resp<UserIdentity>> GetAuthAdmin()
        {
            var key = string.Concat(CoreCacheKeys.Portal_User_ByToken, AppReqContext.Identity.token);
            var user =await CacheHelper.GetAsync<UserIdentity>(key);
            if (user != null)
                return new Resp<UserIdentity>(user);

            var userRes = await RestApiHelper.GetApi<Resp<UserIdentity>>("/b/Portal/GetAuthIdentity");
            if (!userRes.IsSuccess())
            {
                if (userRes.IsRespType(RespTypes.ObjectNull))
                    userRes.ret = (int) RespTypes.UnLogin;

                return new Resp<UserIdentity>().WithResp(userRes);
            }

            await CacheHelper.SetAsync(key, userRes.data,TimeSpan.FromHours(2));
            return userRes;
        }

        public static Task ClearAdminCache()
        {
            var key = string.Concat(CoreCacheKeys.Portal_User_ByToken, AppReqContext.Identity.token);
            return CacheHelper.RemoveAsync(key);
        }

        internal static async Task<Resp> LogOut(HttpContext context)
        {
            ClearCookie(context.Response);

            var userRes = await GetAuthAdmin();
            if (!userRes.IsSuccess())
                return new Resp();

            // 清楚用户权限相关缓存         
            await FuncHelper.ClearAuthUserFuncListCache(userRes.data);
            await ClearAdminCache();

            return new Resp(RespTypes.UnLogin, "请登录！");
        }

        public static void SetCookie(HttpResponse response, string token)
        {
            response.Cookies.Append(CoreConstKeys.UserCookieName, token,
          new CookieOptions() { HttpOnly = true, Expires = DateTimeOffset.Now.AddDays(30) });
        }

        public static string GetCookie(HttpRequest req)
        {
            return req.Cookies[CoreConstKeys.UserCookieName];
        }

        public static void ClearCookie(HttpResponse response)
        {
            response.Cookies.Delete(CoreConstKeys.UserCookieName);
        }



    }
}
