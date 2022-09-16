using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Article.Client;

internal class CategoryHttpClient : ICategoryOpenService
{
    /// <summary>
    ///  查询列表
    /// </summary>
    /// <returns></returns>
    public Task<PageListResp<CategoryMo>> Search(SearchReq req)
    {
          return new ArticleRemoteReq("/Category/Search")
            .PostAsync<PageListResp<CategoryMo>>(req);
    }

    /// <summary>
    ///  通过id获取详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<IResp<CategoryMo>> Get(long id)
    {
          return new ArticleRemoteReq($"/Category/Get?id={id}")
            .GetAsync<IResp<CategoryMo>>();
    }

    public Task<IResp<CategoryMo>> GetUseable(long id)
    {
        return new ArticleRemoteReq($"/Category/GetUseable?id={id}")
            .GetAsync<IResp<CategoryMo>>();
    }


    /// <summary>
    ///  设置可用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag">可用标识 1-可用 ， 0-不可用</param>
    /// <returns></returns>
    public Task<IResp> SetUseable(long id, ushort flag)
    {
          return new ArticleRemoteReq($"/Category/SetUseable?id={id}&flag={flag}")
            .PostAsync<IResp>();
    }

    /// <summary>
    ///  添加对象
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<LongResp> Add(AddCategoryReq req)
    {
          return new ArticleRemoteReq("/Category/Add")
            .PostAsync<LongResp>(req);
    }
}

