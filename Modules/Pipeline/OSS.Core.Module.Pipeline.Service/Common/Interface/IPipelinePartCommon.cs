using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  模块内部服务公用
/// </summary>
public interface IPipelineCommon: IPipelineOpenService
{
    /// <summary>
    ///  获取管道流水线部分信息（如：版本状态）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    internal Task<IResp<PipelinePartMo>> Get(long id);

    /// <summary>
    ///  获取流水线信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    internal Task<IResp<PipelineMo>> GetLine(long id);

}