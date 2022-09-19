using OSS.Common;

namespace OSS.Core.Module.Portal
{
    /// <summary>
    ///  认证中心使用的外部客户端注册启动器
    /// </summary>
    public class PortalGlobalStarter:AppStarter
    {
        /// <inheritdoc />
        public override void Start(IServiceCollection service)
        {
            service.Register<PortalServiceStarter>();    // 逻辑服务层
            service.Register<PortalRepositoryStarter>(); // 仓储层


        }
    }
}
