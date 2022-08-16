using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client.Http;

internal class PortalRequest: BaseCoreRequest
{
    public PortalRequest(string apiPath) : base("Portal", apiPath)
    {
    }
}