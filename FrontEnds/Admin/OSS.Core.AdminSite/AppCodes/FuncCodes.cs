namespace OSS.CorePro.AdminSite.AppCodes
{
    public static class FuncCodes
    {
        public const string None = "none";

        #region Portal
        public const string Portal = "portal";

        //===== 用户
        public const string Portal_UserAdd = "portal_user_add";
        public const string Portal_UserList   = "portal_user_list";
        public const string Portal_UserLock   = "portal_user_lock";
        public const string Portal_UserUnLock = "portal_user_unlock";

        //===== 管理员
        public const string Portal_AdminList   = "portal_admin_list";
        public const string Portal_AdminCreate = "portal_admin_create";
        public const string Portal_AdminLock   = "portal_admin_lock";
        public const string Portal_AdminUnLock = "portal_admin_unlock";
        public const string Portal_AdminSetType = "portal_admin_settype";

        #endregion

        #region Permit
        public const string Permit = "permit";

        //===== role  角色
        public const string Permit_RoleList   = "permit_role_list";
        public const string Permit_RoleAdd    = "permit_role_add";
        public const string Permit_RoleUpdate = "permit_role_update";
        public const string Permit_RoleActive = "permit_role_active";
        public const string Permit_RoleDelete = "permit_role_delete";


        //===== role_func 角色权限项
        public const string Permit_RoleFuncList   = "permit_rolefunc_list"; // 查看关联绑定
        public const string Permit_RoleFuncChange = "permit_rolefunc_change"; // 查看关联绑定

        //===== role_user 角色用户
        public const string Permit_RoleUserSearch = "permit_roleuser_search";
        public const string Permit_RoleUserBind   = "permit_roleuser_bind";
        public const string Permit_RoleUserDelete = "permit_roleuser_delete";

        #endregion


    }
}
