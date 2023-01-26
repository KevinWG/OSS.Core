using Microsoft.AspNetCore.Mvc;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace TM.WMS;

/// <summary>
///  仓库 开放 WebApi 
/// </summary>
public class WarehouseController : BaseWMSController, IWarehouseOpenService
{
    private static readonly IWarehouseOpenService _service = new WarehouseService();

    /// <summary>
    /// 所有仓库列表(不含已作废)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<ListResp<WarehouseFlatItem>> AllUseable()
    {
        return _service.AllUseable();
    }

    /// <summary>
    /// 所有仓库列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [UserFuncMeta(WMSConst.FuncCodes.wms_warehouse)]
    public Task<ListResp<WarehouseFlatItem>> All()
    {
        return _service.All();
    }

    /// <summary>
    ///  获取可用仓库信息(含父级名称)
    /// </summary>
    /// <param name="id">id</param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<WarehouseFlatItem>> GetUseableFlatItem(long id)
    {
        return _service.GetUseableFlatItem(id);
    }

    /// <summary>
    ///  添加仓库对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_warehouse_Manage)]
    public Task<LongResp> Add([FromBody] AddWarehouseReq req)
    {
        return _service.Add(req);
    }

    /// <summary>
    ///  修改仓库信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_warehouse_Manage)]
    public Task<Resp> Update(long id, [FromBody]UpdateWarehouseReq req)
    {
        return _service.Update(id, req);
    }



    /// <summary>
    ///  设置仓库可用状态
    /// </summary>
    /// <param name="id">通过Id生成的通行码</param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_warehouse_Manage)]
    public Task<Resp> SetUseable(long id, ushort flag)
    {
        return _service.SetUseable(id, flag);
    }

    /// <summary>
    ///  添加 库位/区
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_wareArea_Manage)]
    public Task<LongResp> AddArea([FromBody]AddAreaReq req)
    {
        return _service.AddArea(req);
    }

    /// <summary>
    ///  修改 库位/区
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_wareArea_Manage)]
    public Task<Resp> UpdateArea(long id,[FromBody] UpdateAreaReq req)
    {
        return _service.UpdateArea(id, req);
    }



    /// <summary>
    ///  验证库区/位 编码是否可以添加
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Resp> CheckAreaCode([FromBody]CheckAreaCodeReq req)
    {
        return _service.CheckAreaCode(req);
    }


    /// <summary>
    ///   库位/区列表
    /// </summary>
    /// <param name="w_id"></param>
    /// <returns></returns>
    [HttpGet]
    //[UserFuncMeta(WMSConst.FuncCodes.wms_wareArea)]
    public Task<ListResp<WareAreaMo>> AreaList(long w_id)
    {
        return _service.AreaList(w_id);
    }

    /// <summary>
    ///   设置 库位/区 可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_wareArea_Manage)]
    public Task<Resp> SetAreaUseable(long id, ushort flag)
    {
        return _service.SetAreaUseable(id, flag);
    }

    /// <summary>
    ///   设置 区位 的交易标识 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_wareArea_Manage)]
    public Task<Resp> SetAreaTradeFlag(long id, TradeFlag flag)
    {
        return _service.SetAreaTradeFlag(id, flag);
    }
}
