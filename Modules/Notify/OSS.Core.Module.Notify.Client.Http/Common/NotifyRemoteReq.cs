
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Notify.Client;

internal class NotifyRemoteReq: BaseRemoteRequest
{
    public NotifyRemoteReq(string apiPath) : base("Notify", apiPath)
    {
    }


    protected override Task PrepareSendAsync()
    {
        api_path = string.Concat("/",target_module, api_path);
        return base.PrepareSendAsync();
    }
}
