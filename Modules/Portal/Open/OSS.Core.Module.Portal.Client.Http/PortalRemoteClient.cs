using OSS.Common;

namespace OSS.Core.Module.Portal.Client;

public static class PortalRemoteClient 
{
    public static IPermitOpenService Permit { get; } = SingleInstance<PermitClient>.Instance;
    public static IAuthOpenService Auth { get; } = SingleInstance<AuthClient>.Instance;
    public static ISettingOpenService Setting { get; } = SingleInstance<SettingClient>.Instance;
}
