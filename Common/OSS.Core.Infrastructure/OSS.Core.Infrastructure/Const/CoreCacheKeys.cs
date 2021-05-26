namespace OSS.Core.Infrastructure.Const
{
    public static class CoreCacheKeys
    {
        #region Permit

        public const string Perm_Role_ById = "Perm_Role_";
        public const string Perm_RoleFuncs_ByRId = "Perm_RoleFuncs_";

        public const string Perm_UserRoles_ByUId = "Perm_UserRoles_";
        public const string Perm_UserFuncs_ByUId = "Perm_UserFuncs_";

        #endregion

        #region Portal

        /// <summary>
        ///     验证码前缀
        /// </summary>
        public const string Portal_Passcode_ByLoginName = "Portal_Passcode_";

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

        #endregion
        
        #region System_Global

        /// <summary>
        ///  字典配置信息  后缀 {key}
        /// </summary>
        public const string System_Dir_Config_ByKey = "Dir_Config_";

        /// <summary>
        ///  日志key值记录
        /// </summary>
        public const string System_Log_BySourceAndKey = "System_Log_";

        #endregion
    }
}
