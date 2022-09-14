using Microsoft.Extensions.DependencyInjection;
using OSS.Common;
using OSS.Core;

namespace OSS.Core.Module.Article;

/// <summary>
///  服务层启动注入
///       启动注入（内部）相关，注入外部引用项请在全局注入
/// </summary>
public class ArticleServiceStarter : AppStarter
{
    public override void Start(IServiceCollection serviceCollection)
    {
    }
}
