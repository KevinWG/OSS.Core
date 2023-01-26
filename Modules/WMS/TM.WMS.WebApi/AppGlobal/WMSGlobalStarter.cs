using OSS.Core.Comp.DirConfig.Mysql;
using OSS.Common;
using OSS.Core;

namespace TM.WMS;

/// <summary>
///   全局注入
/// </summary>
public class WMSGlobalStarter : AppStarter
{
    /// <inheritdoc />
    public override void Start(IServiceCollection services)
    {
         services.UserMysqlDirConfigTool();

         services.Register<WMSDomainStarter>();    // 领域层启动注入
         services.Register<WMSServiceStarter>();   // 逻辑服务层启动注入        
         
    }
}
