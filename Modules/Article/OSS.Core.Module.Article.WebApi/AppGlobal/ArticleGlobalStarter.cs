using OSS.Core.Comp.DirConfig.Mysql;
using OSS.Common;
using OSS.Core;

namespace OSS.Core.Module.Article;

/// <summary>
///   全局注入
/// </summary>
public class ArticleGlobalStarter : AppStarter
{
    /// <inheritdoc />
    public override void Start(IServiceCollection services)
    {
         services.UserMysqlDirConfigTool();

         services.Register<ArticleDomainStarter>();    // 领域层启动注入
         services.Register<ArticleServiceStarter>();   // 逻辑服务层启动注入        
    }
}
