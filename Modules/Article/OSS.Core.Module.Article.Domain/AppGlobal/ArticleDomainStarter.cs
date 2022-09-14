using Microsoft.Extensions.DependencyInjection;
using OSS.Common;
using OSS.Core;

namespace OSS.Core.Module.Article;

/// <summary>
///  启动注册配置
///    领域层 注入（内部）相关，注入外部引用项请在全局注入
/// </summary>
public class ArticleDomainStarter : AppStarter
{
    public override void Start(IServiceCollection serviceCollection)
    {
    }
}
