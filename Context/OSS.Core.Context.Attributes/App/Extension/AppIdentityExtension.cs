using Microsoft.AspNetCore.Http;

namespace OSS.Core.Context.Attributes
{
    public static class AppIdentityExtension
    {
        /// <summary>
        ///  初始化当前请求上下文的应用全局信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static AppIdentity InitialContextAppIdentity(this HttpContext context)
        {
            var sysInfo = CoreAppContext.Identity;
            if (sysInfo != null) return sysInfo;

            sysInfo = new AppIdentity();

            CoreAppContext.SetIdentity(sysInfo);
            return sysInfo;
        }
    }
}
