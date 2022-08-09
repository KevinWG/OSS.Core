using OSS.Common;

namespace OSS.Core.Module.Notify;

public class NotifyDefaultClient : INotifyClient
{
    public IOpenedNotifyService NotifyService { get; } = InsContainer<INotifyService>.Instance;
}