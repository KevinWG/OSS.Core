namespace OSS.Core.Shared.Domain;

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
    Wechat = 100,

    /// <summary>
    ///     支付宝
    /// </summary>
    Ali = 200,

    /// <summary>
    ///     新浪
    /// </summary>
    Sina = 300
}