
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
///  出入库申请 对象实体 
/// </summary>
public class StockApplyMo : BaseTenantOwnerAndStateMo<long>
{
    /// <summary>
    ///  申请单据名称
    /// </summary>
    public string title { get; set; } = string.Empty;

    /// <summary>
    ///  仓库Id
    /// </summary>
    public long warehouse_id { get; set; }

    /// <summary>
    ///  仓库名称
    /// </summary>
    public string warehouse_name { get; set; } = string.Empty;

    /// <summary>
    ///  公司Id
    /// </summary>
    public long org_id { get; set; }

    /// <summary>
    ///  公司名称
    /// </summary>
    public string? org_name { get; set; }

    /// <summary>
    ///   出入库方向
    /// </summary>
    public StockDirection direction { get; set; }

    /// <summary>
    ///  出入库计划时间
    /// </summary>
    public long plan_time { get; set; }
}


/// <summary>
///  申请状态
/// </summary>
public enum ApplyStatus
{
    /// <summary>
    ///  作废
    /// </summary>
    Abandon = -100,

    /// <summary>
    /// 待确认
    /// </summary>
    WaitConfirm = 0,

    /// <summary>
    /// 待执行
    /// </summary>
    Confirmed = 100,

    /// <summary>
    /// 已处理完成
    /// </summary>
    Done = 200
}


/// <summary>
///  出入口方向
/// </summary>
public enum StockDirection
{
    /// <summary>
    ///  入库
    /// </summary>
    In=10,

    /// <summary>
    ///   出库
    /// </summary>
    Out =20
}