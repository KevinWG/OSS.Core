using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Article;

/// <summary>
///  Article 开放 WebApi 
/// </summary>
public class ArticleController : BaseArticleController, IArticleOpenService
{
    private static readonly IArticleOpenService _service = new ArticleService();

    /// <summary>
    ///  查询Article列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public Task<PageListResp<ArticleMo>> Search([FromBody] SearchReq req)
    {
        return _service.Search(req);
    }

    /// <summary>
    ///  通过id获取Article详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<IResp<ArticleMo>> Get(long id)
    {
        return _service.Get(id);
    }

    /// <summary>
    ///  获取有效可用的文章信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<IResp<ArticleMo>> GetUseable(long id)
    {
        return _service.GetUseable(id);
    }

    /// <summary>
    ///  设置Article可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IResp> SetUseable(long id, ushort flag)
    {
        return await _service.SetUseable(id, flag);
    }

    /// <summary>
    ///  添加Article对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp> Add([FromBody] AddArticleReq req)
    {
        return _service.Add(req);
    }
}
