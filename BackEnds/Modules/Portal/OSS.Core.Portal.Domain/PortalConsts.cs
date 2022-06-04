
namespace OSS.Core.Portal.Domain
{
    public static class PortalConst
    {
        public static class CacheKeys
        {
            /// <summary>
            /// 用户信息 （+ token
            /// </summary>
            public const string Portal_UserIdentity_ByToken = "Portal_UserIdentity_";

            /// <summary>
            ///  用户信息
            /// </summary>
            public const string Portal_User_ById = "Portal_User_";

            /// <summary>
            ///  管理员信息
            /// </summary>
            public const string Portal_Admin_ByUId = "Portal_Admin_";
        }

        public static class DataFlowMsgKeys
        {
            /// <summary>
            ///  用户注册成功事件
            /// </summary>
            public const string Portal_UserReg = "Portal_UserReg";
        }

    }
}
