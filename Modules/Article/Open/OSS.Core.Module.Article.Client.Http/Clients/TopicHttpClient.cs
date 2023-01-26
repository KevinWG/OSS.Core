using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Article.Client;

internal class TopicHttpClient : ITopicOpenService
{
    /// <inheritdoc />
    public Task<TokenPageListResp<TopicMo>> MSearch(SearchReq req)
    {
          return new ArticleRemoteReq("/Topic/MSearch")
            .PostAsync<TokenPageListResp<TopicMo>>(req);
    }

    /// <inheritdoc />
    public Task<Resp<TopicMo>> Get(long id)
    {
          return new ArticleRemoteReq($"/Topic/Get?id={id}")
            .GetAsync<Resp<TopicMo>>();
    }

    /// <inheritdoc />
    public Task<Resp<TopicMo>> GetUseable(long id)
    {
          return new ArticleRemoteReq($"/Topic/GetUseable?id={id}")
            .GetAsync<Resp<TopicMo>>();
    }
    
    /// <inheritdoc />
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
          return new ArticleRemoteReq($"/Topic/SetUseable?pass_token={pass_token}&flag={flag}")
            .PostAsync<Resp>();
    }

    /// <inheritdoc />
    public Task<LongResp> Add(AddTopicReq req)
    {
          return new ArticleRemoteReq("/Topic/Add")
            .PostAsync<LongResp>(req);
    }
}

