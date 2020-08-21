
namespace OSS.Core.RepDapper.Basic.SocialPlats.Mos
{
    /// <summary>
    ///   平台来源类型
    /// </summary>
    public enum SocialPlatform
    {
        /// <summary>
        ///     系统本身
        /// </summary>
        None = 0,

        /// <summary>
        ///     微信公众号
        /// </summary>
        WeChat = 10,

        /// <summary>
        /// 微信小程序
        /// </summary>
        WeChatApp = 11,

        /// <summary>
        ///     支付宝
        /// </summary>
        AliPay = 20,

        /// <summary>
        ///     新浪
        /// </summary>
        Sina = 30
    }
}