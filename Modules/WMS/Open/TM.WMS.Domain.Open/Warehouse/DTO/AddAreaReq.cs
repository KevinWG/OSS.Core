using System.ComponentModel.DataAnnotations;
using OSS.Core.Domain;

namespace TM.WMS;

/// <summary>
///   添加库区/位请求
/// </summary>
public class AddAreaReq: UpdateAreaReq
{
    /// <summary>
    /// 库区/位编号 
    /// </summary>
    [Required(ErrorMessage = "库/区编号不能为空!")]
    [StringLength(30, ErrorMessage = "库/区编号不能超过30个字符")]
    public string code { get; set; } = string.Empty;

    /// <summary>
    ///  仓库id
    /// </summary>
    public long warehouse_id { get; set; }

    ///// <summary>
    ///// 交易标识
    ///// 0-正常，-100 - 不可交易（如：不合格，退货等不可交易区域）
    ///// </summary>
    //public TradeFlag trade_flag { get; set; }
}

/// <summary>
///  更新区位请求
/// </summary>
public class UpdateAreaReq
{
    /// <summary>
    ///  备注
    /// </summary>
    [StringLength(300, ErrorMessage = "区位备注不能超过300个字符")]
    public string? remark { get; set; }
}



/// <summary>
///  库位/区转化映射
/// </summary>
public static class AddAreaReqMap
{
    /// <summary>
    ///  转化为库位/区对象实体
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public static WareAreaMo MapToWareAreaMo(this AddAreaReq req)
    {
        var mo = new WareAreaMo
        {
            code = req.code,
            warehouse_id = req.warehouse_id,
            remark = req.remark,
            trade_flag = TradeFlag.Normal
        };

        mo.FormatBaseByContext();
        return mo;
    }
}
