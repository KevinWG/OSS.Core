using Microsoft.Extensions.DependencyInjection;
using OSS.Common;
using OSS.Core;

namespace TM.Module.Product;

/// <summary>
///  服务层启动注入
///       启动注入（内部）相关，注入外部引用项请在全局注入
/// </summary>
public class ProductServiceStarter : AppStarter
{
    public override void Start(IServiceCollection serviceCollection)
    {
        // 初始化service层内部容器注入等操作
    }
}
