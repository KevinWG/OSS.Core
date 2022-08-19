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
    public async Task<PageListResp<PipeMo>> Search(SearchReq req)
    {
        return new PageListResp<PipeMo>(await _PipeRep.Search(req));
    }

    /// <inheritdoc />
    public Task<IResp<PipeMo>> Get(long id) => _PipeRep.GetById(id);


    /// <inheritdoc />
    public Task<IResp> SetUseable(long id, ushort flag)
    {
        return _PipeRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<IResp> Add(AddPipeReq req)
    {
        var mo = req.MapToPipeMo();
        mo.FormatBaseByContext();

        await _PipeRep.Add(mo);
        return Resp.DefaultSuccess;
    }
}
