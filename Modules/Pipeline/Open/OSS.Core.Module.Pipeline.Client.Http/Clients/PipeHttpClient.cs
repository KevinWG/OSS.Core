using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Pipeline.Client;

internal class PipeHttpClient : IPipeOpenService
{
    /// <inheritdoc />
    public Task<LongResp> AddStart(AddPipeReq req)
    {
        return new PipelineRemoteReq("/Pipeline/Pipe/AddStart")
            .PostAsync<LongResp>(req);
    }

    /// <inheritdoc />
    public Task<LongResp> AddEnd(AddPipeReq req)
    {
        return new PipelineRemoteReq("/Pipeline/Pipe/AddEnd")
            .PostAsync<LongResp>(req);
    }

    /// <inheritdoc />
    public Task<IResp> Delete(long id)
    {
        return new PipelineRemoteReq($"/Pipeline/Pipe/Delete?id={id}")
            .PostAsync<IResp>();
    }
}

