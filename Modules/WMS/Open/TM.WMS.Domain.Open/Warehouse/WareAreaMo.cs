
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
///  库位/区 对象实体 
/// </summary>
public class WareAreaMo : BaseTenantOwnerAndStateMo<long>
{
    /// <summary>
    /// 名称/编号
    /// </summary>
    public string code { get; set; } = string.Empty;

    /// <summary>
    ///  仓库id
    /// </summary>
    public long warehouse_id { get; set; }

    /// <summary>
    ///  备注
    /// </summary>
    public string? remark { get; set; }

    /// <summary>
    /// 交易标识
    /// 0-正常，-100 - 不可交易（如：不合格，退货等不可交易区域）
    /// </summary>
    public TradeFlag trade_flag { get; set; }
}


public enum TradeFlag
{
    /// <summary>
    ///  不可交易(不合格，退货等区域)
    /// </summary>
    UnActive = -100,

    /// <summary>
    /// 正常
    /// </summary>
    Normal = 0,
}