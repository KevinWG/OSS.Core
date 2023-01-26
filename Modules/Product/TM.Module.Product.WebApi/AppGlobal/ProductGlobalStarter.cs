using OSS.Core;
using OSS.Core.Comp.DirConfig.Mysql;

namespace TM.Module.Product;

/// <summary>
///   全局注入
/// </summary>
public class ProductGlobalStarter : AppStarter
{
    /// <inheritdoc />
    public override void Start(IServiceCollection services)
    {
         services.UserMysqlDirConfigTool();

         services.Register<ProductDomainStarter>();    // 领域层启动注入
         services.Register<ProductServiceStarter>();   // 逻辑服务层启动注入        
         
    }
}
