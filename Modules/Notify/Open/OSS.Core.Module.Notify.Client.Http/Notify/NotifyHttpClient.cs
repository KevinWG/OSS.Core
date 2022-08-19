using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Notify.Client;

internal class NotifyHttpClient : INotifyOpenService
{

    public Task<NotifySendResp> Send(NotifySendReq msg)
    {
        return new NotifyRemoteReq("/Notify/Send")
            .PostAsync<NotifySendResp>(msg);
    }
}

