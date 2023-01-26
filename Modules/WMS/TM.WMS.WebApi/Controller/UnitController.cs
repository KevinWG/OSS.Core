using Microsoft.AspNetCore.Mvc;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace TM.WMS;

/// <summary>
///  单位 开放 WebApi 
/// </summary>
public class UnitController : BaseWMSController, IUnitOpenService
{
    private static readonly IUnitOpenService _service = new UnitService();

    /// <summary>
    ///  查询单位列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<ListResp<UnitView>> All()
    {
        return _service.All();
    }

    /// <summary>
    ///  添加单位对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_unit_manage)]
    public Task<LongResp> Add([FromBody] AddUnitReq req)
    {
        return _service.Add(req);
    }
}
