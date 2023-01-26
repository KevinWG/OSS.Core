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
    public Task<Resp<TemplateMo>> Get(long id)
    {
        return new NotifyRemoteReq("/Template/Get?id=" + id)
            .GetAsync<Resp<TemplateMo>>();
    }

    /// <inheritdoc />
    public Task<Resp> Update(long id, AddTemplateReq req)
    {
        return new NotifyRemoteReq("/Template/Update?id=" + id)
            .PostAsync<Resp>(req);
    }

    /// <inheritdoc />
    public Task<Resp> SetUseable(long id, ushort flag)
    {
        return new NotifyRemoteReq(string.Concat("/Template/SetUseable?id=", id, "&flag=", flag))
            .PostAsync<Resp>();
    }

    /// <inheritdoc />
    public Task<Resp> Add(AddTemplateReq req)
    {
        return new NotifyRemoteReq("/Template/Add")
            .PostAsync<Resp>(req);
    }
}