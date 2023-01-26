using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;

namespace TM.WMS;

/// <summary>
///  库存 开放 WebApi 
/// </summary>
public class StockController : BaseWMSController, IStockOpenService
{
    private static readonly IStockOpenService _service = new StockService();


    #region 物料库存（物料维度

    /// <summary>
    ///  库存管理(关联物料信息)搜索列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Task<PageListResp<MaterialStockView>> MSearchUnion([FromBody] SearchReq req)
    {
        return _service.MSearchUnion(req);
    }

    /// <summary>
    ///   获取指定物料库存信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<ListResp<MaterialStockCount>> GetStockList([FromBody] GetStockListReq req)
    {
        return _service.GetStockList(req);
    }

    #endregion


    #region 区位库存（区位+编码 维度


    /// <summary>
    ///   区位库存信息搜索列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Task<PageListResp<AreaStockView>> SearchArea([FromBody] SearchReq req)
    {
        return _service.SearchArea(req);
    }



    #endregion


    /// <summary>
    ///  库存变动（具体到区位
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Resp> StockChange([FromBody]StockChangeReq req)
    {
        return _service.StockChange(req);
    }
}
