using Microsoft.Extensions.DependencyInjection;
using OSS.Common;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  服务层启动注入
///       启动注入（内部）相关，注入外部引用项请在全局注入
/// </summary>
public class PipelineServiceStarter : AppStarter
{
    /// <inheritdoc />
    public override void Start(IServiceCollection serviceCollection)
    {
        InsContainer<IPipeCommon>.Set<PipeService>();
        InsContainer<IPipelineCommon>.Set<PipelineService>();

        InsContainer<IFlowCommon>.Set<FlowService>();
    }
}
