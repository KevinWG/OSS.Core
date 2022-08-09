namespace OSS.Core.Context;

/// <summary>
///  应用类型
///      - 逐级放宽
/// </summary>
public enum AppType
{
    /// <summary>
    ///  平台管理应用
    /// </summary>
    SystemManager = 1,

    /// <summary>
    ///  平台应用
    /// </summary>
    System = 30,

    /// <summary>
    ///  平台上多租户代理应用
    /// </summary>
    Proxy = 60,

    /// <summary>
    ///  独立应用
    /// </summary>
    Single = 80
}