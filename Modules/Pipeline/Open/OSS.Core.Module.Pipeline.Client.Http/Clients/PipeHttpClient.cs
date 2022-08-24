using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Pipeline.Client;

internal class PipeHttpClient : IPipeOpenService
{
    /// <inheritdoc />
    public Task<IResp<long>> Add(AddPipeReq req)
    {
        return new PipelineRemoteReq("/Pipeline/Pipe/Add")
            .PostAsync<IResp<long>>(req);
    }

    /// <inheritdoc />
    public Task<IResp> Delete(long id)
    {
        return new PipelineRemoteReq($"/Pipeline/Pipe/Delete?id={id}")
            .PostAsync<IResp>();
    }
}

