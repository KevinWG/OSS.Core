namespace TM.WMS;

/// <summary>
/// 
/// </summary>
public class AreaStockView
{
    /// <summary>
    /// Id
    /// </summary>
    public long id { get; set; }


    /// <summary>
    /// 物料Id
    /// </summary>
    public long material_id { get; set; }

    /// <summary>
    /// 仓库Id
    /// </summary>
    public long warehouse_id { get; set; }

    /// <summary>
    /// 仓库Id
    /// </summary>
    public string warehouse_name { get; set; } = string.Empty;

    /// <summary>
    ///  仓库-区/位
    /// </summary>
    public long area_id { get; set; }

    /// <summary>
    ///  仓库-区/位
    /// </summary>
    public string area_code { get; set; } = string.Empty;






    #region 批次号信息

    /// <summary>
    /// 批次号Id
    /// </summary>
    public long batch_id { get; set; }

    /// <summary>
    /// 批次号编码
    /// </summary>
    public string batch_code { get; set; } = string.Empty;

    /// <summary>
    /// 批次号过期时间
    /// </summary>
    public long batch_expire_date { get; set; }


    #endregion




    /// <summary> 
    ///  物料单位（同一个物料，进库转化为基础单位保存）
    /// </summary>
    public string unit { get; set; } = string.Empty;

    /// <summary>
    ///  可用数量
    /// </summary>
    public decimal usable_count { get; set; }
}