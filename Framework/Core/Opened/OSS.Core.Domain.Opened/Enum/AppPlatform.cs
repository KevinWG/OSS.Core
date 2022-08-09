namespace OSS.Core.Domain;

/// <summary>
///   应用平台
/// </summary>
public enum AppPlatform
{
    /// <summary>
    ///     系统本身
    /// </summary>
    Self = 0,

    /// <summary>
    ///  微信
    /// </summary>
    Wechat = 10000,

    /// <summary>
    ///     支付宝
    /// </summary>
    Ali = 20000,

    /// <summary>
    ///     新浪
    /// </summary>
    Sina = 30000
}
