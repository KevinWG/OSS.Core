using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Notify.Client;

internal class TemplateHttpClient : ITemplateOpenService
{
    /// <inheritdoc />
    public Task<PageListResp<TemplateMo>> Search(SearchReq req)
    {
        return new NotifyRemoteReq("/Template/Search")
            .PostAsync<PageListResp<TemplateMo>>(req);
    }

    /// <inheritdoc />
    public Task<IResp<TemplateMo>> Get(long id)
    {
        return new NotifyRemoteReq("/Template/Get?id=" + id)
            .GetAsync<IResp<TemplateMo>>();
    }

    /// <inheritdoc />
    public Task<IResp> Update(long id, AddTemplateReq req)
    {
        return new NotifyRemoteReq("/Template/Update?id=" + id)
            .PostAsync<IResp>(req);
    }

    /// <inheritdoc />
    public Task<IResp> SetUseable(long id, ushort flag)
    {
        return new NotifyRemoteReq(string.Concat("/Template/SetUseable?id=", id, "&flag=", flag))
            .PostAsync<IResp>();
    }

    /// <inheritdoc />
    public Task<IResp> Add(AddTemplateReq req)
    {
        return new NotifyRemoteReq("/Template/Add")
            .PostAsync<IResp>(req);
    }
}