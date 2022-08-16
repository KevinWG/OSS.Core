using OSS.Common;

namespace OSS.Core.Module.Notify;

public class LocalNotifyClient : INotifyClient
{
    public IOpenedNotifyService NotifyService { get; } = InsContainer<INotifyService>.Instance;
}