using OSS.Core.Comp.DirConfig.Mysql;
using OSS.Common;
using OSS.Core;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///   全局注入
/// </summary>
public class PipelineGlobalStarter : AppStarter
{
    /// <inheritdoc />
    public override void Start(IServiceCollection services)
    {
         services.UserMysqlDirConfigTool();

         services.Register<PipelineDomainStarter>();    // 领域层启动注入
         services.Register<PipelineServiceStarter>();   // 逻辑服务层启动注入        
         
    }
}
