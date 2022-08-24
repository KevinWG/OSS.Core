using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线 服务逻辑
/// </summary>
public class PipelineService : IPipelineOpenService
{
    private static readonly PipelineMetaRep    _metaRep     = new();
    private static readonly PipelinePartRep _pipelineRep = new();

    /// <inheritdoc />
    public async Task<PageListResp<PipelineView>> Search(SearchReq req)
    {
        var lineMos = await _pipelineRep.SearchLines(req);
        return new PageListResp<PipelineView>(lineMos.total,lineMos.data.Select(x=>x.ToView()).ToList());
    }

    /// <inheritdoc />
    public async Task<List<PipelineView>> GetVersions(long metaId)
    {
        var lineMos = await _pipelineRep.GetVersions(metaId);
        return  lineMos.Select(x => x.ToView()).ToList();
    }

    /// <inheritdoc />
    public async Task<IResp<PipelineDetailView>> GetDetail(long id)
    {
        var pRes = await _pipelineRep.GetLine(id);
        return new Resp<PipelineDetailView>().WithResp(pRes, p => p.ToDetailView());
    }

    /// <inheritdoc />
    public Task<IResp> Publish(long id)
    {
        return _pipelineRep.UpdateStatus(id, PipelineStatus.Published);
    }

    /// <inheritdoc />
    public Task<IResp> TurnOff(long id)
    {
        return _pipelineRep.UpdateStatus(id, PipelineStatus.OffLine);
    }

    /// <inheritdoc />
    public async Task<IResp> Delete(long id)
    {
        var lineRes = await _pipelineRep.GetById(id);
        if (lineRes.IsSuccess())
            return lineRes;

        var line = lineRes.data;
        if (line.status != PipelineStatus.Original)
            return new Resp(RespCodes.OperateFailed, "当前版本已经发布(发布过)，不能删除!");

        return  await _pipelineRep.UpdateStatus(id, PipelineStatus.Deleted);
    }


    #region 添加流水线
    
    /// <inheritdoc />
    public async Task<IResp> Add(AddPipelineReq req)
    {
        var pipe = new PipeMo()
        {
            name = req.name, type = PipeType.Pipeline
        };
        await InsContainer<IPipeCommon>.Instance.AddPipe(pipe);

        await CreateMeta(pipe.id);
        await CreateMetaVersion(pipe.id);

        return new LongResp(pipe.id);
    }

    private static async Task CreateMeta(long pipeId)
    {
        var metaMo = new MetaMo();

        metaMo.latest_pipe_id = metaMo.id = pipeId;// 初始metaId = 第一版pipeId
        metaMo.FormatBaseByContext();

        await _metaRep.Add(metaMo);
    }

    private static async Task<PipelinePartMo> CreateMetaVersion(long pipeId)
    {
        var verMo = new PipelinePartMo
        {
            id      = pipeId,
            meta_id = pipeId
        };

        verMo.FormatBaseByContext();
        await _pipelineRep.Add(verMo);

        return verMo;
    }

    #endregion
}
