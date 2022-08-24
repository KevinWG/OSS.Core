using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线 服务逻辑
/// </summary>
public partial class PipeService
{
    /// <inheritdoc />
    public Task<LongResp> AddSubPipeline(AddPipeReq req)
    {
        var mo = req.MapToPipeMo(PipeType.SubPipeline);

        return AddPipe(mo);
    }

}