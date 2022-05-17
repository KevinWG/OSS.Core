
namespace OSS.Core.Portal.Domain
{
    internal static class PortalConst
    {
        public static class CacheKeys
        {
            /// <summary>
            ///     验证码前缀
            /// </summary>
            public const string Portal_Passcode_ByLoginName = "Portal_Passcode_";

            /// <summary>
            ///    绑定账号信息时老账号验证码
            /// </summary>
            public const string Portal_Bind_Passcode_Old_ByType = "Portal_Bind_Passcode_Old_";

            /// <summary>
            ///    绑定账号信息时新账号验证码
            /// </summary>
            public const string Portal_Bind_Passcode_New_ByName = "Portal_Bind_Passcode_New_";

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


            /// <summary>
            /// 二维码登录Code对应的用户Id  ( +QRCode
            /// </summary>
            public const string Portal_UserId_ByQRCode = "Portal_UserId_";
            
        }
    }
}
