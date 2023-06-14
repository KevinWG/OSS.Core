namespace OSS.Core.Context;

/// <summary>
///  应用类型
///      - 逐级放宽
/// </summary>
public enum AppType
{
    /// <summary>
    ///  平台内部管理应用（多租户）
    /// </summary>
    SystemManager = 1,

    /// <summary>
    ///  平台内部应用（单租户）
    /// </summary>
    System = 30,

    /// <summary>
    ///  平台外部多租户代理应用 (多租户)
    /// </summary>
    Proxy = 60,

    /// <summary>
    ///  独立应用
    /// </summary>
    Single = 80
}