using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Pipeline.Client;

internal class PipelineHttpClient : IPipelineOpenService
{
    /// <inheritdoc />
    public Task<IResp> Add(AddPipelineReq req)
    {
          return new PipelineRemoteReq($"/Pipeline/Pipeline/Add")
            .PostAsync<IResp>(req);
    }

    /// <inheritdoc />
    public Task<PageListResp<PipelineView>> Search(SearchReq req)
    {
        return new PipelineRemoteReq("/Pipeline/pipeline/SearchLines")
            .PostAsync<PageListResp<PipelineView>>(req);
    }

    /// <inheritdoc />
    public Task<List<PipelineView>> GetVersions(long metaId)
    {
        return new PipelineRemoteReq($"/Pipeline/pipeline/GetVersions?meta_id={metaId}")
            .GetAsync<List<PipelineView>>();
    }

    /// <inheritdoc />
    public Task<IResp<PipelineDetailView>> GetDetail(long id)
    {
        return new PipelineRemoteReq($"/Pipeline/pipeline/GetDetail?id={id}")
            .GetAsync<IResp<PipelineDetailView>>();
    }

    /// <inheritdoc />
    public Task<IResp> Publish(long id)
    {
        return new PipelineRemoteReq($"/Pipeline/pipeline/Publish?id={id}")
            .PostAsync<IResp>();
    }
    /// <inheritdoc />
    public Task<IResp> TurnOff(long id)
    {
        return new PipelineRemoteReq($"/Pipeline/pipeline/TurnOff?id={id}")
            .PostAsync<IResp>();
    }

    /// <inheritdoc />
    public Task<IResp>      Delete(long id)
    {
        return new PipelineRemoteReq($"/Pipeline/pipeline/Delete?id={id}")
            .PostAsync<IResp>();
    }
}

