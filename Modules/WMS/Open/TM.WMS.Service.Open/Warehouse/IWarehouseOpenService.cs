using OSS.Common;
using OSS.Common.Resp;

namespace TM.WMS;

/// <summary>
///  Warehouse 领域对象开放接口
/// </summary>
public interface IWarehouseOpenService
{
    /// <summary>
    /// 所有仓库列表(不包含已作废)
    /// </summary>
    /// <returns></returns>
    Task<ListResp<WarehouseFlatItem>> AllUseable();


    /// <summary>
    /// 所有仓库列表（含已作废）
    /// </summary>
    /// <returns></returns>
    Task<ListResp<WarehouseFlatItem>> All();

    /// <summary>
    ///  获取可用仓库信息(含父级名称)
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    Task<Resp<WarehouseFlatItem>> GetUseableFlatItem(long id);

    /// <summary>
    ///  设置仓库可用状态
    /// </summary>
    /// <param name="id">通过Id生成的通行码</param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    Task<Resp> SetUseable(long id, ushort flag);

    /// <summary>
    ///  添加仓库对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> Add(AddWarehouseReq req);

    /// <summary>
    ///  修改仓库信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<Resp> Update(long id,UpdateWarehouseReq req);

    /// <summary>
    ///  添加 库位/区
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<LongResp> AddArea(AddAreaReq req);


    /// <summary>
    ///  修改 库位/区
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<Resp> UpdateArea(long id, UpdateAreaReq req);



    /// <summary>
    ///  验证库区/位 编码是否可以添加
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<Resp> CheckAreaCode(CheckAreaCodeReq req);

    /// <summary>
    ///   库位/区列表
    /// </summary>
    /// <param name="w_id"></param>
    /// <returns></returns>
    Task<ListResp<WareAreaMo>> AreaList(long w_id);

    /// <summary>
    ///   设置 库位/区 可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    Task<Resp> SetAreaUseable(long id, ushort flag);

    /// <summary>
    ///   设置 区位 的交易标识 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    Task<Resp> SetAreaTradeFlag(long id, TradeFlag flag);
}
