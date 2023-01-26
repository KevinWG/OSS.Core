using Microsoft.AspNetCore.Mvc;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace TM.Module.Product;

/// <summary>
///  产品分类 开放 WebApi 
/// </summary>
public class SpuCategoryController : BaseProductController, ISpuCategoryOpenService
{
    private static readonly ISpuCategoryOpenService _service = new SpuCategoryService();
    /// <summary>
    ///  获取当前 产品分类 信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<SpuCategoryMo>> Get(long id)
    {
        return _service.Get(id);
    }


    /// <summary>
    ///  产品所有分类
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<ListResp<SpuCategoryTreeItem>> AllCategories()
    {
        return _service.AllCategories();
    }

    /// <summary>
    /// 添加产品分类
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(ProductConst.FuncCodes.Product_SpuCategory_Manage)]
    public Task<LongResp> Add([FromBody] AddSpuCategoryReq req)
    {
        return _service.Add(req);
    }

    /// <summary>
    ///  设置 产品分类 可用状态
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(ProductConst.FuncCodes.Product_SpuCategory_Manage)]
    public Task<Resp> SetUseable(long id, ushort flag)
    {
        return _service.SetUseable(id, flag);
    }

    /// <summary>
    /// 编辑 产品分类 名称
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(ProductConst.FuncCodes.Product_SpuCategory_Manage)]
    public Task<Resp> UpdateName(long id, [FromBody] UpdateSCNameReq req)
    {
        return _service.UpdateName(id, req);
    }

    /// <summary>
    /// 编辑 产品分类 排序值
    /// </summary>
    /// <param name="id"></param>
    /// <param name="order">排序值</param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(ProductConst.FuncCodes.Product_SpuCategory_Manage)]
    public Task<Resp> UpdateOrder(long id, int order)
    {
        return _service.UpdateOrder(id, order);
    }
}
