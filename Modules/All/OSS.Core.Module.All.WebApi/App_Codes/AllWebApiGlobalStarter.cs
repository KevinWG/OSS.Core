using OSS.Core.Comp.DirConfig.Mysql;
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
        
        services.Register<NotifyGlobalStarter>(); //  注册对应配置信息
        services.Register<PortalGlobalStarter>(); // 认证中心服务层
    }
}