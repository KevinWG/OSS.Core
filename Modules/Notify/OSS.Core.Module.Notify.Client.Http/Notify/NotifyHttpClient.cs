using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Notify.Client;

internal class NotifyHttpClient : IOpenedNotifyService
{

    public Task<NotifyResp> Send(Notify.NotifyReq msg)
    {
        return new NotifyRemoteReq($"/Notify/Send")
            .PostAsync<NotifyResp>(msg);
    }
}

