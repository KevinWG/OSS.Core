
#region Copyright (C)  Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 实体对象
*
*　　	创建人： osscore
*    	创建日期：
*       
*****************************************************************************/

#endregion

using OSS.Core.Domain;

namespace TM.WMS;


/// <summary>
///  库存 对象实体
///     同一物料，需要转化为基础单位保存
/// </summary>
public class AreaStockMo : BaseTenantOwnerMo<long>
{
    /// <summary>
    /// 物料Id
    /// </summary>
    public long material_id { get; set; }

    /// <summary>
    /// 仓库Id
    /// </summary>
    public long warehouse_id { get; set; }

    /// <summary>
    ///  仓库-区/位
    /// </summary>
    public long area_id { get; set; }



    #region 批次号信息，固定不变，冗余

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

public static class AreaStockMap
{
    /// <summary>
    /// 转化为视图模型
    /// </summary>
    /// <param name="mo"></param>
    /// <returns></returns>
    public static AreaStockView ToView(this AreaStockMo mo)
    {
        return new AreaStockView()
        {
            id           = mo.id,
            material_id  = mo.material_id,
            warehouse_id = mo.warehouse_id,
            area_id      = mo.area_id,

            batch_id          = mo.batch_id,
            batch_expire_date = mo.batch_expire_date,
            batch_code        = mo.batch_code,

            unit         = mo.unit,
            usable_count = mo.usable_count
        };
    }



    /// <summary>
    ///  转化为库存项实体
    /// </summary>
    /// <param name="record"></param>
    /// <param name="material"></param>
    /// <returns></returns>
    public static AreaStockMo ToStock(this StockRecordMo record, MaterialView material)
    {
        var mo = new AreaStockMo
        {
            material_id  = record.material_id,
            warehouse_id = record.warehouse_id,
            usable_count = record.change_basic_count,

            area_id  = record.area_id,
            batch_id = record.batch_id,
            unit     = material.basic_unit,
        };

        mo.FormatBaseByContext();
        return mo;
    }
}