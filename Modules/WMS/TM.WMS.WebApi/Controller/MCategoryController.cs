using Microsoft.AspNetCore.Mvc;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace TM.WMS;

/// <summary>
///  物料目录 开放 WebApi 
/// </summary>
public class MCategoryController : BaseWMSController, IMCategoryOpenService
{
    private static readonly IMCategoryOpenService _service = new MCategoryService();

    /// <summary>
    ///  获取当前物料目录信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<MCategoryMo>> Get(long id)
    {
        return _service.Get(id);
    }

    /// <summary>
    ///  获取所有物料目录
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<ListResp<MCategoryTreeItem>> AllCategories()
    {
        return _service.AllCategories();
    }


    /// <summary>
    /// 添加物料目录
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_mcategory_manage)]
    public Task<LongResp> Add([FromBody] AddCategoryReq req)
    {
        return _service.Add(req);
    }

    /// <summary>
    ///  设置物料目录可用状态
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_mcategory_manage)]
    public Task<Resp> SetUseable(long id, ushort flag)
    {
        return _service.SetUseable(id, flag);
    }

    /// <summary>
    /// 编辑物料目录
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_mcategory_manage)]
    public Task<Resp> UpdateName(long id, [FromBody] UpdateMCategoryNameReq req)
    {
        return _service.UpdateName(id, req);
    }

    /// <summary>
    /// 编辑 物料目录 排序值
    /// </summary>
    /// <param name="id"></param>
    /// <param name="order">排序值</param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(WMSConst.FuncCodes.wms_mcategory_manage)]
    public Task<Resp> UpdateOrder(long id, int order)
    {
        return _service.UpdateOrder(id, order);
    }
}
