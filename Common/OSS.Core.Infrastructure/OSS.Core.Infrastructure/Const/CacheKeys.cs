namespace OSS.Core.Infrastructure.Const
{
   public static class CacheKeys
   {
        #region tenant

        public const string Tenant_Info_ById = "Tenant_Info_";

        #endregion
        
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
        ///  用户信息
        /// </summary>
        public const string Portal_User_ById = "Portal_User_";
        public const string Portal_User_ByToken = "Portal_User_";

        /// <summary>
        ///  管理员信息
        /// </summary>
        public const string Portal_Admin_ByUId = "Portal_Admin_";
        #endregion


        #region Plugs

        /// <summary>
        ///  配置信息缓存前缀
        /// </summary>
        public const string Plugs_Config_ByKey = "Plugs_Config_";

        #endregion

    }
}
