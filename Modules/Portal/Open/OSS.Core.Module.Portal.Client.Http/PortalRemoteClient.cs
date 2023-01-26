using OSS.Common;

namespace OSS.Core.Module.Portal.Client;

public static class PortalRemoteClient 
{
    /// <summary>
    ///  Organization 接口
    /// </summary>
    public static IOrganizationOpenService Organization {get; } = SingleInstance<OrganizationHttpClient>.Instance;
    public static IGrantOpenService Grant { get; } = SingleInstance<PermitHttpClient>.Instance;
    public static IAuthOpenService Auth { get; } = SingleInstance<AuthHttpClient>.Instance;
    public static ISettingOpenService Setting { get; } = SingleInstance<SettingHttpClient>.Instance;
}

