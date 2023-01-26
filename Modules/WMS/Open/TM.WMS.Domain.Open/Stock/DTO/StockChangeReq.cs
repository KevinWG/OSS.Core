using OSS.Core.Domain;

namespace TM.WMS;

/// <summary>
///   添加库存变动记录请求
/// </summary>
public class StockChangeReq
{
    /// <summary>
    /// 物料Id
    /// </summary>
    public long material_id { get; set; }

    /// <summary>
    /// 批次号Id
    /// </summary>
    public long batch_id { get; set; }

    /// <summary>
    ///  目标仓库-区/位
    /// </summary>
    public long area_id { get; set; }





    /// <summary>
    ///  数量变动来源
    /// </summary>
    public StockBusinessType b_type { get; set; }

    /// <summary>
    ///  变动来源主键Id
    /// </summary>
    public long b_id { get; set; }

    /// <summary>
    ///  操作Id
    ///    b_type+b_id,确定业务单据信息，
    ///     operate_id 为业务单据下操作Id，同一业务单据下不可重复
    /// </summary>
    public long operate_id { get; set; }




    /// <summary>
    ///  变动数量
    /// </summary>
    public int change_count { get; set; }

    /// <summary>
    ///  变动的物料单位
    /// </summary>
    public string change_unit { get; set; }= string.Empty;
}

/// <summary>
///  
/// </summary>
public static class AddStockRecordReqMap
{
    /// <summary>
    ///  转化为库存项明细记录实体
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public static StockRecordMo ToRecord(this StockChangeReq req)
    {
        var mo = new StockRecordMo
        {
            operate_id  = req.operate_id,
            material_id = req.material_id,
            area_id     = req.area_id,

            batch_id = req.batch_id,

            b_type = req.b_type,
            b_id   = req.b_id,

            change_count = req.change_count,
            change_unit  = req.change_unit
        };

        mo.FormatBaseByContext();

        if (mo.operate_id == 0)
            mo.operate_id = mo.id;

        return mo;
    }

}
