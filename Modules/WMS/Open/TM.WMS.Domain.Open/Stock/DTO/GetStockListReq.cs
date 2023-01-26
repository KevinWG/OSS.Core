namespace TM.WMS;

/// <summary>
///  获取指定物料库存列表
/// </summary>
public class GetStockListReq
{
    /// <summary>
    /// 物料Id列表
    /// </summary>
    public List<long>? m_ids { get; set; }

    /// <summary>
    /// 指定仓库，默认0-不指定
    /// </summary>
    public long warehouse_id { get; set; }
}