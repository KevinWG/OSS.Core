using OSS.Common;

namespace OSS.Core.Module.Portal.Client;

public static class PortalRemoteClient 
{
    public static IOpenedPermitService Permit { get; } = SingleInstance<PermitClient>.Instance;
    public static IOpenedAuthService Auth { get; } = SingleInstance<AuthClient>.Instance;
    public static IOpenedSettingService Setting { get; } = SingleInstance<SettingClient>.Instance;
}
