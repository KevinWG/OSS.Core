using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client;

internal class PortalRequest: BaseRemoteRequest
{
    public PortalRequest(string apiPath) : base("Portal", apiPath)
    {
    }
}