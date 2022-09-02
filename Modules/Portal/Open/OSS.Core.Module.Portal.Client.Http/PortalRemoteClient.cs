using OSS.Common;

namespace OSS.Core.Module.Portal.Client;

public static class PortalRemoteClient 
{
    public static IGrantOpenService Grant { get; } = SingleInstance<PermitHttpClient>.Instance;
    public static IAuthOpenService Auth { get; } = SingleInstance<AuthHttpClient>.Instance;
    public static ISettingOpenService Setting { get; } = SingleInstance<SettingHttpClient>.Instance;
}
