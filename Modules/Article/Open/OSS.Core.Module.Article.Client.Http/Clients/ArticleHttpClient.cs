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
    public Task<IResp<ArticleMo>> Get(long id)
    {
          return new ArticleRemoteReq($"/Article/Article/Get?id={id}")
            .GetAsync<IResp<ArticleMo>>();
    }


    /// <inheritdoc />
    public Task<IResp<ArticleMo>> GetUseable(long id)
    {
        return new ArticleRemoteReq($"/Article/Article/GetUseable?id={id}")
            .GetAsync<IResp<ArticleMo>>();
    }


    /// <summary>
    ///  设置可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    public Task<IResp> SetUseable(long id, ushort flag)
    {
          return new ArticleRemoteReq($"/Article/Article/SetUseable?id={id}&flag={flag}")
            .PostAsync<IResp>();
    }

    /// <summary>
    ///  添加对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<IResp> Add(AddArticleReq req)
    {
          return new ArticleRemoteReq("/Article/Article/Add")
            .PostAsync<IResp>(req);
    }
}

