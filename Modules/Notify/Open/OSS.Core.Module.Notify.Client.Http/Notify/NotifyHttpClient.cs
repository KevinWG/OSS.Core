using OSS.Core.Client.Http;

namespace OSS.Core.Module.Notify.Client;

internal class NotifyHttpClient : INotifyOpenService
{
    /// <inheritdoc />
    public Task<NotifySendResp> Send(NotifySendReq msg)
    {
        return new NotifyRemoteReq("/Notify/Send")
            .PostAsync<NotifySendResp>(msg);
    }
}