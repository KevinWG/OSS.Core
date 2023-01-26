using OSS.Common;
using OSS.Common.Resp;

namespace TM.WMS;

/// <summary>
///  Stock 领域对象开放接口
/// </summary>
public interface IStockOpenService
{
    /// <summary>
    ///  库存管理(关联物料信息)搜索列表
    /// </summary>
    /// <returns></returns>
    Task<PageListResp<MaterialStockView>> MSearchUnion(SearchReq req);

    /// <summary>
    ///   获取指定物料库存信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<ListResp<MaterialStockCount>> GetStockList(GetStockListReq req);




    /// <summary>
    ///   区位库存信息搜索列表
    /// </summary>
    /// <returns></returns>
    Task<PageListResp<AreaStockView>> SearchArea(SearchReq req);




    /// <summary>
    ///  库存变动
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<Resp> StockChange(StockChangeReq req);
}