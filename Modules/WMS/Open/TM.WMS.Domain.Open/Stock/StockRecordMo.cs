
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
///  出入库明细 对象实体 
/// </summary>
public class StockRecordMo : BaseTenantOwnerMo<long>
{
    /// <summary>
    ///  操作Id（唯一，不能重复）
    /// </summary>
    public long operate_id { get; set; }

    /// <summary>
    ///  库存项Id
    /// </summary>
    public long area_stock_id { get; set; }


    /// <summary>
    /// 物料Id
    /// </summary>
    public long material_id { get; set; }

    /// <summary>
    /// 批次号Id
    /// </summary>
    public long batch_id { get; set; }

    /// <summary>
    ///  仓库Id
    /// </summary>
    public long warehouse_id { get; set; }



    /// <summary>
    ///  仓库-区/位
    /// </summary>
    public long area_id { get; set; }




    /// <summary>
    /// 物料编号
    /// </summary>
    public string material_code { get; set; } = string.Empty;

    /// <summary>
    /// 物料名称
    /// </summary>
    public string material_name { get; set; } = string.Empty;





    /// <summary>
    ///  数量变动来源
    /// </summary>
    public StockBusinessType b_type { get; set; }

    /// <summary>
    ///  变动来源数据Id
    /// </summary>
    public long b_id { get; set; }


    /// <summary>
    ///  变动数量
    /// </summary>
    public decimal change_count { get; set; }

    /// <summary>
    ///  变动的物料单位
    /// </summary>
    public string change_unit { get; set; } = string.Empty;

    /// <summary>
    ///  变动数量(对应基础单位)
    /// </summary>
    public decimal change_basic_count { get; set; }


    /// <summary>
    ///  变动前对应数量(基础单位)
    /// </summary>
    public decimal before_basic_count { get; set; }
}

/// <summary>
///  变动来源
/// </summary>
public enum StockBusinessType
{
    /// <summary>
    ///  库存初始化
    /// </summary>
    Initial = 0,

    /// <summary>
    ///  采购
    /// </summary>
    Purchase = 100,

    /// <summary>
    ///  采购退货
    /// </summary>
    PurchaseRefund = 110,

    /// <summary>
    ///  销售（订单）
    /// </summary>
    Sale = 200,

    /// <summary>
    ///  退货
    /// </summary>
    SaleRefund = 210,

    /// <summary>
    /// 生产计划
    /// </summary>
    Produce = 300,

    /// <summary>
    ///  调（划）拨
    /// </summary>
    Allot = 500,

    /// <summary>
    ///  直接变动申请
    /// </summary>
    Apply = 600
}


public static class BusinessTypeExtension
{
    /// <summary>
    ///  可以占用临时库存的业务类型
    /// </summary>
    /// <param name="bType"></param>
    /// <returns></returns>
    public static bool CanBlockStock(this StockBusinessType bType)
    {
        return bType == StockBusinessType.Produce;
    }

    /// <summary>
    ///  是否是交易类型
    /// </summary>
    /// <param name="bType"></param>
    /// <returns></returns>
    public static bool IsTradeType(this StockBusinessType bType)
    {
        return bType == StockBusinessType.Purchase || bType == StockBusinessType.Sale;
    }
}