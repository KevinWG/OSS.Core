namespace OSS.Core.Module.Portal;

public  static class PortalConst
{
    public const string ModuleName = "portal";


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


        #region Permit

        public const string Permit_Func_All = "Permit_Func_All";

        public const string Permit_Role_ById = "Permit_Role_";
        public const string Permit_RoleFuncs_ByRId = "Permit_RoleFuncs_";

        public const string Permit_UserRoles_ByUId = "Permit_UserRoles_";
        public const string Permit_UserFuncs_ByUId = "Permit_UserFuncs_";
        #endregion

    }

    public static class DataFlowMsgKeys
    {
        /// <summary>
        ///  微信公众号的扫码订阅事件
        /// </summary>
        public const string Chat_Wechat_SubScan = "Chat_Wechat_SubScan";
    }


    public static class CookieKeys
    {
        /// <summary>
        ///     用户验证cookie名称
        /// </summary>
        public const string UserCookieName = "u_cn";
    }


    public static class DirConfigKeys
    {
        public const string auth_setting = ModuleName+"_auth_setting";
    }

    public static class FuncCodes
    {
        public const string portal_auth_setting =  "portal_auth_setting";
        public const string portal_func_operate =  "portal_func_operate";

        public const string portal_user_add    = "portal_user_add";
        public const string portal_user_list   = "portal_user_list";
        public const string portal_user_lock   = "portal_user_lock";
        public const string portal_user_unlock = "portal_user_unlock";
        
        //===== 管理员
        public const string portal_admin_list    = "portal_admin_list";
        public const string portal_admin_create  = "portal_admin_create";
        public const string portal_admin_lock    = "portal_admin_lock";
        public const string portal_admin_unlock  = "portal_admin_unlock";
        public const string portal_admin_settype = "portal_admin_settype";

        //===== role_func 角色权限项
        public const string portal_grant_role_permits = "portal_grant_role_permits"; // 查看关联绑定
        public const string portal_grant_role_change  = "portal_grant_role_change";  // 修改角色绑定权限项信息




        //===== role  角色

        public const string portal_role_list   = "portal_role_list";
        public const string portal_role_add    = "portal_role_add";
        public const string portal_role_active = "portal_role_active";

        public const string portal_user_roles       = "portal_user_roles";
        public const string portal_role_bind_user   = "portal_role_bind_user";
        public const string portal_role_bind_delete = "portal_role_bind_delete";


    
        //===== role_user 角色用户
        public const string Permit_RoleUserSearch = "permit_roleuser_search";

   
    }
}