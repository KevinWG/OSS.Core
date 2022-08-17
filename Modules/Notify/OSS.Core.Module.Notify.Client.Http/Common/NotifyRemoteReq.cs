
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Notify.Client;

internal class NotifyRemoteReq: BaseRemoteRequest
{
    public NotifyRemoteReq(string apiPath) : base("Notify", apiPath)
    {
    }
}
