using OSS.Common;
using OSS.Common.Resp;

namespace TM.WMS;

/// <summary>
///  StockApply 领域对象开放接口
/// </summary>
public interface IStockApplyOpenService
{
    /// <summary>
    ///  出入库申请管理搜索列表（附带通过Id生成的PassToken）
    /// </summary>
    /// <returns></returns>
    Task<TokenPageListResp<StockApplyMo>> MSearch(SearchReq req);

    /// <summary>
    ///  通过id获取出入库申请详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Resp<StockApplyMo>> Get(long id);

    /// <summary>
    ///  通过id获取有效可用状态的出入库申请详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Resp<StockApplyMo>> GetUseable(long id);

    /// <summary>
    ///  设置出入库申请可用状态
    /// </summary>
    /// <param name="pass_token">通过Id生成的通行码</param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<Resp> SetUseable(string pass_token, ushort flag);

    /// <summary>
    ///  添加出入库申请对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> Add(AddStockApplyReq req);

    /// <summary>
    ///  修改出入库申请对象
    /// </summary>
    /// <param name="pass_token">通过Id生成的通行码</param>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<Resp> Edit(string pass_token, AddStockApplyReq req);

}
