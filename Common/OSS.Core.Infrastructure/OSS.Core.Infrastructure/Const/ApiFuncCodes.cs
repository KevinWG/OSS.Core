namespace OSS.Core.Infrastructure.Const
{
    public static class ApiFuncCodes
    {
        public const string None = "none";

        #region Portal
        public const string Portal = "portal";

        //===== 用户
        public const string Portal_User_Add = "portal_user_add";
        public const string Portal_User_List = "portal_user_list";
        public const string Portal_User_Lock = "portal_user_lock";
        public const string Portal_User_UnLock = "portal_user_unlock";

        //===== 管理员
        public const string Portal_Admin_List = "portal_admin_list";
        public const string Portal_Admin_Create = "portal_admin_create";
        public const string Portal_Admin_Lock = "portal_admin_lock";
        public const string Portal_Admin_UnLock = "portal_admin_unlock";
        public const string Portal_Admin_SetType = "portal_admin_settype";

        #endregion



        #region Permit
        public const string Permit = "permit";

        //===== role  角色
        public const string Permit_RoleList = "permit_role_list";
        public const string Permit_RoleAdd = "permit_role_add";
        public const string Permit_RoleUpdate = "permit_role_update";
        public const string Permit_RoleActive = "permit_role_active";
        public const string Permit_RoleDelete = "permit_role_delete";


        //===== role_func 角色权限项
        public const string Permit_RoleFuncList = "permit_rolefunc_list"; // 查看关联绑定
        public const string Permit_RoleFuncChange = "permit_rolefunc_change"; // 查看关联绑定

        //===== role_user 角色用户
        public const string Permit_RoleUserSearch = "permit_roleuser_search";
        public const string Permit_RoleUserBind = "permit_roleuser_bind";
        public const string Permit_RoleUserDelete = "permit_roleuser_delete";

        #endregion



        #region Goods

        public const string Goods = "good";

        public const string Goods_Category_List = "goods_category_list";
        public const string Goods_Category_Add = "goods_category_add";
        public const string Goods_Category_Delete = "goods_category_delete";

        public const string Goods_Add = "goods_add";
        public const string Goods_Edit = "goods_edit";
        public const string Goods_Price_Manage = "goods_price_manage";
        public const string Goods_Delete = "goods_delete";

        public const string Goods_Config_Attr_Search = "goods_config_attr_search";
        public const string Goods_Config_Attr_Manage = "goods_config_attr_manage";

        public const string Goods_Sku_Manage = "goods_sku_manage";


        #endregion

        public const string Article = "article";

        public const string Article_Category_List = "article_category_list";
        public const string Article_Category_Add = "article_category_add";
        public const string Article_Category_Delete = "article_category_delete";

        public const string Article_Add = "article_add";
        public const string Article_Edit = "article_edit";
        public const string Article_Delete = "article_delete";




        public const string System_Setting = "system_setting";


        public const string System_Home_Index_Setting = "system_home_index_setting";


        #region Notify


        public const string Notify_Account_Manage = "notify_account_manage";

        public const string Notify_Template_Manage = "notify_template_manage";

        #endregion

    }
}
