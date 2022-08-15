using OSS.Common;
using OSS.Core.Module.Notify;
using OSS.Core.Module.Portal;

namespace OSS.Core.Module.All.WebApi;

/// <summary>
///   WebApi全局注册入口
/// </summary>
public class AllWebUsedClientStarter : AppStarter
{
    public override void Start(IServiceCollection services)
    {
        InsContainer<INotifyClient>.Set<NotifyDefaultClient>();

        InsContainer<IPortalClient>.Set<LocalPortalClient>();
    }
}