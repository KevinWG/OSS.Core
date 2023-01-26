using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace TM.WMS;

/// <summary>
///  物料 开放 WebApi 
/// </summary>
public class MaterialController : BaseWMSController, IMaterialOpenService
{
    private static readonly IMaterialOpenService _service = new MaterialService();

    /// <summary>
    ///  查询物料列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    //[UserFuncMeta(WMSConst.FuncCodes.wms_material)]
    public Task<PageListResp<MaterialView>> MSearch([FromBody] SearchReq req)
    {
        return _service.MSearch(req);
    }


    /// <summary>
    ///  通过id获取物料详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<MaterialView>> Get(long id)
    {
        return _service.Get(id);
    }

    /// <summary>
    ///   通过id获取 有效可用的 物料 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<MaterialView>> GetUseable(long id)
    {
        return _service.GetUseable(id);
    }
    
    /// <summary>
    ///  设置物料可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_material_manage)]
    public Task<Resp> SetUseable(long id, ushort flag)
    {
        return _service.SetUseable(id, flag);
    }



    /// <summary>
    ///  添加物料对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_material_manage)]
    public Task<LongResp> Add([FromBody] AddMaterialReq req)
    {
        return _service.Add(req);
    }

    /// <summary>
    ///  修改物料对象
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_material_manage)]
    public Task<Resp> Update(long id, [FromBody] UpdateMaterialReq req)
    {
        return _service.Update(id, req);
    }
}
