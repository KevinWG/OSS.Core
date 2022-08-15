using OSS.Common;
using OSS.Core;

namespace OSS.Core.Module.WorkFlow;

/// <summary>
///   全局注入
/// </summary>
public class WorkFlowGlobalStarter : AppStarter
{
    public override void Start(IServiceCollection services)
    {
        // 全局的注入，特别是相关外部依赖信息
    }
}
