using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Article;

/// <summary>
///  Category 开放 WebApi 
/// </summary>
public class CategoryController : BaseArticleController, ICategoryOpenService
{
    private static readonly ICategoryOpenService _service = new CategoryService();

    /// <summary>
    ///  查询Category列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Task<PageListResp<CategoryMo>> Search([FromBody] SearchReq req)    {

        return _service.Search(req);
    }

    /// <summary>
    ///  通过id获取Category详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<CategoryMo>> Get(long id)
    {
        return _service.Get(id);
    }

    /// <summary>
    ///  获取有效可用的分类信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<CategoryMo>> GetUseable(long id)
    {
        return _service.GetUseable(id);
    }

    /// <summary>
    ///  设置Category可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Resp> SetUseable(long id, ushort flag)
    {
        return await _service.SetUseable(id, flag);
    }



    /// <summary>
    ///  添加Category对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<LongResp> Add([FromBody] AddArticleCategoryReq req)
    {
        return _service.Add(req);
    }
}
