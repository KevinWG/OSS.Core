using System.ComponentModel.DataAnnotations;
using OSS.Core.Domain;

namespace TM.WMS;

/// <summary>
///   添加出入库申请请求
/// </summary>
public class AddStockApplyReq
{
    /// <summary>
    ///  申请单据名称
    /// </summary>
    [StringLength(200,ErrorMessage = "标题不能超过200个字符")]
    public string title { get; set; } = string.Empty;

    /// <summary>
    ///  仓库Id
    /// </summary>
    public long warehouse_id { get; set; }
    
    /// <summary>
    ///  公司Id
    /// </summary>
    public long org_id { get; set; }
    
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
///  出入库申请转化映射
/// </summary>
public static class AddStockApplyReqMap
{
    /// <summary>
    ///  转化为出入库申请对象实体
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public static StockApplyMo MapToStockApplyMo(this AddStockApplyReq req)
    {
        var mo = new StockApplyMo();

        mo.FormatByReq(req);
        mo.FormatBaseByContext();

        return mo;
    }

    public static void FormatByReq(this StockApplyMo mo, AddStockApplyReq req)
    {
        mo.title        = req.title;
        mo.warehouse_id = req.warehouse_id;
        mo.org_id       = req.org_id;
        mo.direction    = req.direction;
        mo.plan_time    = req.plan_time;
    }


}
