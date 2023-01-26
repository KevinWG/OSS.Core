namespace TM.WMS;

/// <summary>
///  WMS 模块静态声明
/// </summary>
public static class WMSConst
{
    /// <summary>
    ///  仓储管理模块名称
    /// </summary>
    public const string ModuleName = "wms";


    public static class CacheKeys
    {
        // 放置相关缓存Key
        // 涉及相关动态参数，建议以 ByPara 结尾，例如： WMSDetailById = "WMSDetail_"
        public const string WMS_Warehouse_All = "wms_warehouse_all";
    }

    
    public static class DataMsgKeys
    {
        // 放置 发布订阅/异步 消息key
    }

    /// <summary>
    /// 表名
    /// </summary>
    public static class Tables
    {
        /// <summary>
        /// 库位库存表
        /// </summary>
        public const string area_stock = "wms_area_stock";

        /// <summary>
        ///  物料表
        /// </summary>
        public const string material = "wms_material";

        /// <summary>
        ///  物料表
        /// </summary>
        public const string batch = "wms_batches";

    }

    public static class FuncCodes
    {
        // 放置权限码

        public const string wms_mcategory_manage = "wms_mcategory_manage";

        public const string wms_warehouse        = "wms_warehouse";
        public const string wms_warehouse_Manage = "wms_warehouse_manage";

        //public const string wms_wareArea         = "wms_warearea";
        public const string wms_wareArea_Manage  = "wms_warearea_manage";

        //public const string wms_material        = "wms_material";
        public const string wms_material_manage = "wms_material_manage";

        public const string wms_stock_apply_list   = "wms_stock_apply_list";
        public const string wms_stock_apply_manage = "wms_stock_apply_manage";

        //public const string wms_unit_list   = "wms_unit_list";
        public const string wms_unit_manage = "wms_unit_manage";




        public const string wms_batch_msearch = "wms_batch_msearch";
        public const string wms_batch_modify = "wms_batch_modify";

    }
}

