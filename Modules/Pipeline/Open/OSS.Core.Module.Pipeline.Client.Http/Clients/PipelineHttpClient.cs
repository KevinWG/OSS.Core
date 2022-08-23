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
    public Task<PageListResp<PipelineView>> SearchLines(SearchReq req)
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
}

