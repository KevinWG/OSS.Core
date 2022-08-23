using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Pipe 服务
/// </summary>
public class PipelineService : IPipelineOpenService
{
    private static readonly PipeRep _PipeRep = new();

    private static readonly PipelineVersionRep _versionRep = new();

    /// <inheritdoc />
    public async Task<PageListResp<PipelineView>> SearchLines(SearchReq req)
    {
        var lineMos = await _versionRep.SearchLines(req);
        return new PageListResp<PipelineView>(lineMos.total,lineMos.data.Select(x=>x.ToView()).ToList());
    }

    ///// <inheritdoc />
    //public Task<IResp<PipelineDetailView>> GetPipelineDetail(long id)
    //{

    //}


    /// <inheritdoc />
    public Task<IResp> SetUseable(long id, ushort flag)
    {
        return _PipeRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }


    #region 添加流程

    /// <inheritdoc />
    public async Task<IResp> Add(AddPipeReq req)
    {
        var mo = req.MapToPipeMo();

        mo.FormatBaseByContext();
        mo.execute_ext = "{}";

        if (mo.type == PipeType.Pipeline)
        {
            var meta = await CreateMeta(mo);
            await CreateMetaVersion(mo.id, meta.id);
        }

        await _PipeRep.Add(mo);
        return Resp.DefaultSuccess;
    }

    private static readonly PipelineMetaRep     _metaRep     = new();
    private static readonly PipelineVersionRep _pipelineRep = new();
    private static async Task<MetaMo> CreateMeta(PipeMo pipe)
    {
        var metaMo = new MetaMo();

        metaMo.latest_pipe_id = metaMo.id = pipe.id;

        metaMo.FormatBaseByContext();

        await _metaRep.Add(metaMo);
        return metaMo;
    }

    private static async Task CreateMetaVersion(long pipeId, long metaId)
    {
        var verMo = new VersionMo
        {
            id      = pipeId,
            meta_id = metaId
        };

        verMo.FormatBaseByContext();

        await _pipelineRep.Add(verMo);
    }

    #endregion
}
