using OSS.Common;

namespace OSS.Core.Module.Portal;

public class LocalPortalClient : IPortalClient
{
    /// <inheritdoc />
    public IOpenedPermitService Permit { get; } = SingleInstance<PermitService>.Instance;

    /// <inheritdoc />
    public IOpenedSettingService Setting { get; } = SingleInstance<SettingService>.Instance;

    /// <inheritdoc />
    public IOpenedAuthService Auth { get; } = SingleInstance<AuthService>.Instance;

}