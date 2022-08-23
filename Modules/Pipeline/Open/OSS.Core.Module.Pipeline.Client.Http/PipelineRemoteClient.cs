using OSS.Common;

namespace OSS.Core.Module.Pipeline.Client;

/// <summary>
/// Pipeline 模块接口客户端
/// </summary>
public static class PipelineRemoteClient //: IPipelineClient
{
    /// <summary>
    ///  流水线业务流程接口
    /// </summary>
    public static IFlowOpenService Flow {get; } = SingleInstance<FlowHttpClient>.Instance;

    /// <summary>
    ///  流水线管道接口
    /// </summary>
    public static IPipeOpenService Pipe {get; } = SingleInstance<PipeHttpClient>.Instance;

    /// <summary>
    ///  流水线接口
    /// </summary>
    public static IPipelineOpenService Pipeline { get; } = SingleInstance<PipelineHttpClient>.Instance;
}





