using OSS.Common;

namespace OSS.Core.Module.Pipeline.Client;

/// <summary>
/// Pipeline 模块接口客户端
/// </summary>
public static class PipelineRemoteClient //: IPipelineClient
{

    /// <summary>
    ///  Flow 接口
    /// </summary>
    public static IFlowOpenService Flow {get; } = SingleInstance<FlowHttpClient>.Instance;

    /// <summary>
    ///  Pipe 接口
    /// </summary>
    public static IPipeOpenService Pipe {get; } = SingleInstance<PipeHttpClient>.Instance;
}





