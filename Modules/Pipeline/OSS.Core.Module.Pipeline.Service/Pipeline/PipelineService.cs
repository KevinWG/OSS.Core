using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Pipe 服务
/// </summary>
public class PipelineService : IPipelineOpenService
{
    private static readonly PipelineMetaRep    _metaRep     = new();
    private static readonly PipelineVersionRep _versionRep = new();

    /// <inheritdoc />
    public async Task<PageListResp<PipelineView>> SearchLines(SearchReq req)
    {
        var lineMos = await _versionRep.SearchLines(req);
        return new PageListResp<PipelineView>(lineMos.total,lineMos.data.Select(x=>x.ToView()).ToList());
    }

    ///// <inheritdoc />
    //public Task<IResp> SetUseable(long id, ushort flag)
    //{
    //    return _versionRep.UpdateStatus(id, flag == 1 ? PipelineStatus.Published : PipelineStatus.OffLine);
    //}


    #region 添加流程
    
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

    private static async Task<VersionMo> CreateMetaVersion(long pipeId)
    {
        var verMo = new VersionMo
        {
            id      = pipeId,
            meta_id = pipeId
        };

        verMo.FormatBaseByContext();
        await _versionRep.Add(verMo);

        return verMo;
    }

    #endregion
}
