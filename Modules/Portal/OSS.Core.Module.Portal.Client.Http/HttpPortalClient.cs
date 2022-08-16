using OSS.Common;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client.Http;

public class HttpPortalClient : IPortalClient
{
    public IOpenedPermitService Permit { get; } = SingleInstance<PermitClient>.Instance;
    public IOpenedAuthService Auth { get; } = SingleInstance<AuthClient>.Instance;
    public IOpenedSettingService Setting { get; } = SingleInstance<SettingClient>.Instance;
}

internal class PortalRequest: BaseCoreRequest
{
    public PortalRequest(string apiPath) : base("Portal", apiPath)
    {
    }
}