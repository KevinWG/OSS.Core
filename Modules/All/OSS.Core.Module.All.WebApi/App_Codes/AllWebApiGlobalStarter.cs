using OSS.Core.Comp.DirConfig.Mysql;
using OSS.Core.Module.Article;
using OSS.Core.Module.Notify;
using OSS.Core.Module.Portal;

namespace OSS.Core.Module.All.WebApi;

/// <summary>
///   WebApi全局注册入口
/// </summary>
public class AllWebApiGlobalStarter : AppStarter
{
    public override void Start(IServiceCollection services)
    {
        services.UserMysqlDirConfigTool();

        services.Register<NotifyGlobalStarter>();  //  通知模块
        services.Register<PortalGlobalStarter>();  //  认证门户
        services.Register<ArticleGlobalStarter>(); //  文章中心
    }
}