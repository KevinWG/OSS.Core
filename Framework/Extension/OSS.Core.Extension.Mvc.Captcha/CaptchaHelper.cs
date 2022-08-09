namespace OSS.Core.Extension.Mvc.Captcha
{
    public static class CaptchaHelper
    {
        /// <summary>
        ///  默认验证码实现 提供者
        /// </summary>
        public static ICaptchaValidatorProvider? ValidatorProvider { get; set; }

        /// <summary>
        ///  默认验证码实现
        /// </summary>
        public static ICaptchaValidator? DefaultValidator { get; set; }

    }
}
