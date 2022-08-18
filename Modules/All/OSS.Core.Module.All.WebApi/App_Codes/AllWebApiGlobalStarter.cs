﻿using OSS.Core.Comp.DirConfig.Mysql;
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

        #region Notify（通知服务模块）

        services.Register<NotifyServiceStarter>(); // 通知服务
        services.Register<NotifyGlobalStarter>();   //  注册对应配置信息

        #endregion

        #region Portal（认证服务模块）

        services.Register<PortalRepositoryStarter>();   // 认证中心仓储
        services.Register<PortalServiceStarter>();      // 认证中心服务层
        
        #endregion
    }
}