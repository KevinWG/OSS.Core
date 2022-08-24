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
    public Task<LongResp> AddAudit(AddPipeReq req)
    {
        return new PipelineRemoteReq("/Pipeline/Pipe/AddAudit")
            .PostAsync<LongResp>(req);
    }

    /// <inheritdoc />
    public Task<IResp> SetAuditExe(long id, AuditExt ext)
    {
        return new PipelineRemoteReq("/Pipeline/Pipe/SetAuditExe?id="+ id)
            .PostAsync<IResp>(ext);
    }

    /// <inheritdoc />
    public Task<LongResp> AddSubPipeline(AddPipeReq req)
    {
        return new PipelineRemoteReq("/Pipeline/Pipe/AddSubPipeline")
            .PostAsync<LongResp>(req);
    }

    /// <inheritdoc />
    public Task<IResp> SetSubPipelineExe(long id, SubPipeLineExt ext)
    {
        return new PipelineRemoteReq("/Pipeline/Pipe/SetSubPipelineExe?id=" + id)
            .PostAsync<IResp>(ext);
    }

    /// <inheritdoc />
    public Task<IResp> Delete(long id)
    {
        return new PipelineRemoteReq($"/Pipeline/Pipe/Delete?id={id}")
            .PostAsync<IResp>();
    }
}

