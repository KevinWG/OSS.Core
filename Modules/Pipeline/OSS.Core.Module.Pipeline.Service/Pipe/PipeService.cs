using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线节点服务
/// </summary>
public partial class PipeService : IPipeOpenService, IPipeCommon
{
    private static readonly PipeRep _pipeRep = new();
    
    /// <inheritdoc />
    public Task<LongResp> AddStart(AddPipeReq req)
    {
        var mo = req.MapToPipeMo(PipeType.Start);

        return AddPipe(mo);
    }

    /// <inheritdoc />
    public Task<LongResp> AddEnd(AddPipeReq req)
    {
        var mo = req.MapToPipeMo(PipeType.End);

        return AddPipe(mo);
    }

    /// <inheritdoc />
    public  Task<IResp> Delete(long id)
    {
        return ChangePipe(id, () => _pipeRep.SoftDeleteById(id));
    }


    #region 辅助方法

    private static async Task<IResp> ChangePipe(long id,Func<Task<IResp>> changeHandler)
    {
        var pipeRes = await _pipeRep.GetById(id);
        if (!pipeRes.IsSuccess())
            return new Resp().WithResp(pipeRes, "节点不存在或异常!");

        var pipelineRes = await InsContainer<IPipelinePartCommon>.Instance.Get(pipeRes.data.parent_id);
        if (!pipelineRes.IsSuccess())
            return new Resp().WithResp(pipelineRes, "节点所在流水线不存在或异常!");

        var pipeline = pipelineRes.data;
        return pipeline.status != PipelineStatus.Original 
            ? new Resp(RespCodes.OperateFailed, "节点所在流水线处于不可修改状态!") 
            : await changeHandler();
    }

    private static async Task<LongResp> AddPipe(PipeMo pipe)
    {
        pipe.FormatBaseByContext();
        pipe.execute_ext = "{}";

        await _pipeRep.Add(pipe);

        return new LongResp(pipe.id);
    }


    #endregion


    #region 服务层内部共用

    /// <inheritdoc />
    Task<LongResp> IPipeCommon.AddPipe(PipeMo pipe)
    {
        return AddPipe(pipe);
    }

    /// <inheritdoc />
    async Task<ListResp<PipeView>> IPipeCommon.GetSubPipeViews(long pipelineId)
    {
        var pMoList = await _pipeRep.GetListByParentId(pipelineId);
        return new ListResp<PipeView>(pMoList.Select(p => p.ToView()).ToList());
    }

    #endregion


}