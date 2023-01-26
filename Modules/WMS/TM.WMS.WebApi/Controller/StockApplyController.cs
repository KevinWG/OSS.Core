using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace TM.WMS;

/// <summary>
///  出入库申请 开放 WebApi 
/// </summary>
public class StockApplyController : BaseWMSController, IStockApplyOpenService
{
    private static readonly IStockApplyOpenService _service = new StockApplyService();

    /// <summary>
    ///  查询出入库申请列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_stock_apply_list)]
    public Task<TokenPageListResp<StockApplyMo>> MSearch([FromBody] SearchReq req)
    {
        return _service.MSearch(req);
    }

    /// <summary>
    ///  通过id获取出入库申请详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<StockApplyMo>> Get(long id)
    {
        return _service.Get(id);
    }

    /// <summary>
    ///   通过id获取 有效可用的 出入库申请 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<StockApplyMo>> GetUseable(long id)
    {
        return _service.GetUseable(id);
    }

    /// <summary>
    ///  设置出入库申请可用状态
    /// </summary>
    /// <param name="pass_token"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_stock_apply_manage)]
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
        return _service.SetUseable(pass_token, flag);
    }

    /// <summary>
    ///  添加出入库申请对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_stock_apply_manage)]
    public Task<LongResp> Add([FromBody] AddStockApplyReq req)
    {
        return _service.Add(req);
    }

    /// <summary>
    ///  修改出入库申请对象
    /// </summary>
    /// <param name="pass_token">通过Id生成的通行码</param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_stock_apply_manage)]
    public Task<Resp> Edit(string pass_token, [FromBody] AddStockApplyReq req)
    {
        return _service.Edit(pass_token, req);
    }
}
