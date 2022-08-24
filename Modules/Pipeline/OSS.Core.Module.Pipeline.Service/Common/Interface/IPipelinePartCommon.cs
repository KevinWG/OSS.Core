using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

internal interface IPipelinePartCommon
{
    /// <summary>
    ///  获取管道流水线部分信息（如：版本状态）
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    internal Task<IResp<PipelinePartMo>> Get(long id);
}