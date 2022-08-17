using OSS.Common;
using OSS.Core.Module.Portal;
using OSS.Core.Module.Portal.Client.Http;

namespace OSS.Core.Module.Notify;

/// <summary>
///  通知模块涉及秘钥配置等处理
/// </summary>
public class NotifyGlobalStarter : AppStarter
{
    public override void Start(IServiceCollection serviceCollection)
    {
        InsContainer<IPortalClient>.Set<HttpPortalClient>();
    }
}
