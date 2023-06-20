namespace OSS.Core.Context;

/// <summary>
///  租户认证信息
/// </summary>
public class TenantIdentity
{
    /// <summary>
    ///  租户Id
    /// </summary>
    public string id { get; set; } = string.Empty;

    /// <summary>
    ///  租户类型
    /// </summary>
    public TenantType type { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string name { get; set; } = string.Empty;

    /// <summary>
    ///  logo 图像
    /// </summary>
    public string logo { get; set; } = string.Empty;
}

/// <summary>
///  租户类型
/// </summary>
public enum TenantType
{
    /// <summary>
    ///  平台系统自身
    /// </summary>
    SystemPlatform = 1,

    /// <summary>
    ///  普通租户
    /// </summary>
    Normal = 100
}