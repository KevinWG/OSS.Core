using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线节点服务
/// </summary>
public class PipeService : IPipeOpenService, IPipeCommon
{
    private static readonly PipeRep _pipeRep = new();

    /// <inheritdoc />
    public async Task<IResp<long>> Add(AddPipeReq req)
    {
        if (req.type == PipeType.Pipeline)
        {
            return new LongResp().WithResp(RespCodes.OperateFailed, "无法在流水线下直接创建新的流水线!");
        }

        var mo = req.MapToPipeMo();

        await AddPipe(mo);
        return new LongResp(mo.id);
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

    private static Task AddPipe(PipeMo pipe)
    {
        pipe.FormatBaseByContext();
        pipe.execute_ext = "{}";

        return _pipeRep.Add(pipe);
    }


    #endregion


    #region 服务层内部共用

    /// <inheritdoc />
    Task IPipeCommon.AddPipe(PipeMo pipe)
    {
        return AddPipe(pipe);
    }


    #endregion


}