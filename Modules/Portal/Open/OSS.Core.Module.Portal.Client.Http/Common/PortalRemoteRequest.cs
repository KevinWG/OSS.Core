using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client;

internal class PortalRemoteRequest: BaseRemoteRequest
{
    public PortalRemoteRequest(string apiPath) : base("Portal", apiPath)
    {
    }

    protected override Task PrepareSendAsync()
    {
        api_path = string.Concat("/", target_module, api_path);
        return base.PrepareSendAsync();
    }
}