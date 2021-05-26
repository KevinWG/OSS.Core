using OSS.Core.Context.Helper;
using OSS.Core.Context.Mos;

namespace OSS.Core.Context
{
    /// <summary>
    ///  应用请求上下文
    /// </summary>
    public static class CoreAppContext
    {
        /// <summary>
        ///   成员身份信息
        /// </summary>
        public static AppIdentity Identity
            => ContextHelper.GetContext()?.AppIdentity;

        /// <summary>
        ///   设置用户信息
        /// </summary>
        /// <param name="info"></param>
        public static void SetIdentity(AppIdentity info)
        {
            ContextHelper.SetAppIdentity(info);
        }
    }
}
