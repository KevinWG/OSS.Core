namespace TM.WMS;

/// <summary>
///  物料库存（物料维度）
/// </summary>
public class MaterialStockView: MaterialStockCount
{
    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; } = string.Empty;

    /// <summary>
    ///   物料编码
    /// </summary>
    public string code { get; set; } = string.Empty;

    /// <summary>
    ///  物料形态
    /// </summary>
    public MaterialType type { get; set; }

    /// <summary>
    ///  单位(基础)
    /// </summary>
    public string basic_unit { get; set; } = string.Empty;

    /// <summary>
    ///  涉及区位数量
    /// </summary>
    public int area_count { get; set; }

    /// <summary>
    ///  涉及批次数量
    /// </summary>
    public int batch_count { get; set; }
}


/// <summary>
///  物料库存信息
/// </summary>
public class MaterialStockCount
{  
    /// <summary>
    /// 物料Id
    /// </summary>
    public long material_id { get; set; }

    /// <summary>
    ///  可用数量
    /// </summary>
    public decimal usable_count { get; set; }
}
