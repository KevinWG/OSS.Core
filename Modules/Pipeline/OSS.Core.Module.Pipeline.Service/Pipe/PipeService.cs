using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Pipe 服务
/// </summary>
public class PipeService : IPipeOpenService
{
    private static readonly PipeRep _PipeRep = new();

    /// <inheritdoc />
    public Task<IResp> SetUseable(long id, ushort flag)
    {
        return _PipeRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

}
