using OSS.Common;

namespace OSS.Core.Module.Portal.Client.Http;

public class HttpPortalClient : IPortalClient
{
    public IOpenedPermitService Permit { get; } = SingleInstance<PermitClient>.Instance;
    public IOpenedAuthService Auth { get; } = SingleInstance<AuthClient>.Instance;
    public IOpenedSettingService Setting { get; } = SingleInstance<SettingClient>.Instance;
}
