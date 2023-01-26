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
    public Task<TokenPageListResp<ArticleMo>> MSearch([FromBody] SearchReq req)
    {
        return _service.MSearch(req);
    }

    /// <summary>
    ///  查询文章列表
    /// </summary>
    /// <param name="req"></param>
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
    public Task<Resp<ArticleMo>> Get(long id)
    {
        return _service.Get(id);
    }

    /// <summary>
    ///  获取有效可用的文章信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<Resp<ArticleMo>> GetUseable(long id)
    {
        return _service.GetUseable(id);
    }

    /// <summary>
    ///  删除文章信息
    /// </summary>
    /// <param name="pass_token"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Resp> Delete(string pass_token)
    {
        return _service.Delete(pass_token);
    }


    /// <summary>
    /// 修改编辑文章
    /// </summary>
    /// <param name="pass_token"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Resp> Edit(string pass_token, [FromBody] AddArticleReq req)
    {
        return _service.Edit(pass_token, req);
    }

    /// <summary>
    ///  添加Article对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<LongResp> Add([FromBody] AddArticleReq req)
    {
        return _service.Add(req);
    }


    /// <summary>
    /// 关联专题
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<Resp> RelateTopics([FromBody] RelateTopicReq req)
    {
        return _service.RelateTopics(req);
    }
}
