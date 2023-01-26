using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Article.Client;

internal class ArticleHttpClient : IArticleOpenService
{
    /// <summary>
    ///  查询列表
    /// </summary>
    /// <returns></returns>
    public Task<TokenPageListResp<ArticleMo>> MSearch(SearchReq req)
    {
          return new ArticleRemoteReq("/Article/MSearch")
            .PostAsync<TokenPageListResp<ArticleMo>>(req);
    }

    /// <summary>
    ///  文章查询列表
    /// </summary>
    /// <returns></returns>
    public Task<PageListResp<ArticleMo>> Search(SearchReq req)
    {
        return new ArticleRemoteReq("/Article/Search")
            .PostAsync<PageListResp<ArticleMo>>(req);
    }

    /// <summary>
    ///  通过id获取详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<Resp<ArticleMo>> Get(long id)
    {
          return new ArticleRemoteReq($"/Article/Get?id={id}")
            .GetAsync<Resp<ArticleMo>>();
    }


    /// <inheritdoc />
    public Task<Resp<ArticleMo>> GetUseable(long id)
    {
        return new ArticleRemoteReq($"/Article/GetUseable?id={id}")
            .GetAsync<Resp<ArticleMo>>();
    }

    /// <summary>
    ///  删除
    /// </summary>
    /// <param name="pass_token"></param>
    /// <returns></returns>
    public Task<Resp> Delete(string pass_token)
    {
        return new ArticleRemoteReq($"/Article/Delete?pass_token={pass_token}")
            .PostAsync<Resp>();
    }

    /// <inheritdoc />
    public Task<Resp> Edit(string pass_token, AddArticleReq req)
    {
        return new ArticleRemoteReq($"/Article/Edit?pass_token={pass_token}")
            .PostAsync<Resp>(req);
    }

    /// <summary>
    ///  添加对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<LongResp> Add(AddArticleReq req)
    {
          return new ArticleRemoteReq("/Article/Add")
            .PostAsync<LongResp>(req);
    }

    /// <inheritdoc />
    public Task<Resp> RelateTopics(RelateTopicReq req)
    {
        return new ArticleRemoteReq("/Article/RelateTopics")
            .PostAsync<Resp>(req);
    }
}

