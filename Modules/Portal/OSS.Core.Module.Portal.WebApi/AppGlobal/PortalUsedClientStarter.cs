using OSS.Common;

namespace OSS.Core.Module.Portal
{
    /// <summary>
    ///  认证中心使用的外部客户端注册启动器
    /// </summary>
    public class PortalUsedClientStarter:AppStarter
    {
        /// <inheritdoc />
        public override void Start(IServiceCollection serviceCollection)
        {
            InsContainer<IPortalClient>.Set<PortalDefaultClient>(); // 因为授权是全局中间件调用，虽然在自己的项目里，使用自己的默认client

            // todo 添加通知模块和应用模块 HttpClient (暂未实现)
        }
    }
}
