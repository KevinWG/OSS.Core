namespace OSS.Core.Module.Portal;

public interface IPortalClient
{
    /// <summary>
    ///  权限服务接口
    /// </summary>
    public IOpenedPermitService Permit { get; }

    /// <summary>
    ///    授权服务接口
    /// </summary>
    public IOpenedAuthService Auth { get; }

    /// <summary>
    ///  配置服务接口
    /// </summary>
    public IOpenedSettingService Setting { get; }

}
