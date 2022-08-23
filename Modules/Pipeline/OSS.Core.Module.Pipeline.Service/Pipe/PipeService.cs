using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Pipe 服务
/// </summary>
public class PipeService : IPipeOpenService, IPipeCommon
{
    private static readonly PipeRep _PipeRep = new();

    /// <inheritdoc />
    public Task<IResp> SetUseable(long id, ushort flag)
    {
        return _PipeRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }


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

    private  static  Task AddPipe(PipeMo pipe)
    {
        pipe.FormatBaseByContext();
        pipe.execute_ext = "{}";

        return _PipeRep.Add(pipe);
    }

    /// <inheritdoc />
    Task IPipeCommon.AddPipe(PipeMo pipe)
    {
        return AddPipe(pipe);
    }

}


internal interface IPipeCommon
{
    /// <summary>
    ///  添加管道
    /// </summary>
    /// <param name="newPipe"></param>
    /// <returns></returns>
    internal Task AddPipe(PipeMo newPipe);
}