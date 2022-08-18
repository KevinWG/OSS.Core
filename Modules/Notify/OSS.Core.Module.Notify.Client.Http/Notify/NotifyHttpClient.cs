using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Notify.Client;

internal class NotifyHttpClient : IOpenedNotifyService
{

    public Task<NotifySendResp> Send(NotifySendReq msg)
    {
        return new NotifyRemoteReq("/notify/Notify/Send")
            .PostAsync<NotifySendResp>(msg);
    }
}

