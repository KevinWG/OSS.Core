using Microsoft.Extensions.DependencyInjection;
using OSS.Common;

namespace OSS.Core.Extension.Mvc.Captcha
{
    public static class MvcExtension
    {
        /// <summary>
        ///  使用无需验证的（验证码）验证器
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultNoneCaptchaValidator(this IServiceCollection services)
        {
            CaptchaHelper.DefaultValidator = SingleInstance<DefaultNoneCaptchaValidator>.Instance;
            return services;
        }

        /// <summary>
        ///  使用无需验证的（验证码）验证器
        /// </summary>
        /// <param name="services"></param>
        /// <param name="validator">验证器实现</param>
        /// <returns></returns>
        public static IServiceCollection AddCaptchaValidator(this IServiceCollection services,ICaptchaValidator validator)
        {
            CaptchaHelper.DefaultValidator = validator;
            return services;
        }
        

        /// <summary>
        ///  使用无需验证的（验证码）验证器
        /// </summary>
        /// <param name="services"></param>
        /// <param name="provider">验证器实现提供者</param>
        /// <returns></returns>
        public static IServiceCollection AddCaptchaValidatorProvider(this IServiceCollection services, ICaptchaValidatorProvider provider)
        {
            CaptchaHelper.ValidatorProvider = provider;
            return services;
        }

    }
}
