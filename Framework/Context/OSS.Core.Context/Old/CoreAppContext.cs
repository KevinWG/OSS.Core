
using System;

namespace OSS.Core.Context
{
    /// <summary>
    ///  应用请求上下文
    /// </summary>
    [Obsolete]
    public static class CoreAppContext
    {
        /// <summary>
        ///   应用授权身份信息
        /// </summary>
        public static AppIdentity Identity => CoreContext.App.Identity;

        /// <summary>
        ///   设置应用授权身份信息
        /// </summary>
        /// <param name="info"></param>
        public static void SetIdentity(AppIdentity info)
        {
            CoreContext.App.Identity = info;
        }
    }
}
