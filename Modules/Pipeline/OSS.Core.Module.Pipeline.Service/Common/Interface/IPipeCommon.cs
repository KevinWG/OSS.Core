using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

internal interface IPipeCommon
{
    /// <summary>
    ///  添加管道
    /// </summary>
    /// <param name="newPipe"></param>
    /// <returns></returns>
    internal Task AddPipe(PipeMo newPipe);

    /// <summary>
    ///  获取流水线所有子节点
    /// </summary>
    /// <returns></returns>
    internal Task<ListResp<PipeView>> GetSubPipeViews(long pipelineId);
}