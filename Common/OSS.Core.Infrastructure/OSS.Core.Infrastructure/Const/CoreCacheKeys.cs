namespace OSS.Core.Infrastructure.Const
{
    public static class CoreCacheKeys
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


        #region Region

        /// <summary>
        ///  子区域列表 （+ 父级Id
        /// </summary>
        public const string Region_List_ByPId = "Region_List_";

        /// <summary>
        ///  地区信息 （+ Id
        /// </summary>
        public const string Region_Info_ById = "Region_Info_";

        #endregion


        #region Goods

        /// <summary>
        ///  商品信息
        /// </summary>
        public const string Core_Goods_Goods_ById = "Core_Goods_Goods_";

        /// <summary>
        ///  商品SKU信息  ( + SkuId
        /// </summary>
        public const string Core_Goods_Sku_BySkuId = "Core_Goods_Sku_";

        /// <summary>
        ///  商品SKU信息  ( + 商品Id
        /// </summary>
        public const string Core_Goods_Sku_List_ByGId = "Core_Goods_Sku_";

        /// <summary>
        ///  购物车列表（+ userid
        /// </summary>
        public const string Core_Goods_Goods_Carts_ByUId = "Core_Goods_Goods_Carts_";

        #endregion


        #region Home

        /// <summary>
        ///  日志key值记录
        /// </summary>
        public const string Home_Category = "Home_Category";
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
