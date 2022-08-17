using OSS.Common;

namespace OSS.Core.Module.Notify.Client;

/// <summary>
/// Notify 模块客户端
/// </summary>
public static class NotifyRemoteClient //: INotifyClient
{
    /// <summary>
    ///  Notify 接口
    /// </summary>
    public static IOpenedNotifyService Notify {get; } = SingleInstance<NotifyHttpClient>.Instance;
}


