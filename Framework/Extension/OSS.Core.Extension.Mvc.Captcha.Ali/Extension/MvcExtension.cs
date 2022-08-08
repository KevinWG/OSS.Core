using Microsoft.Extensions.DependencyInjection;
using OSS.Common;

namespace OSS.Core.Extension.Mvc.Captcha;

public static class MvcExtension
{
    /// <summary>
    ///  使用无需验证的（验证码）验证器
    /// </summary>
    /// <param name="services"></param>
    /// <param name="aliSecretProvider">阿里云秘钥提供者(默认从配置文件 Captcha:AliYun 节点获取)</param>
    /// <returns></returns>
    public static IServiceCollection AddAliCaptchaValidator(this IServiceCollection services,
                                                            IAccessProvider<AliCaptchaSecret>? aliSecretProvider = null)
    {
        if (aliSecretProvider != null)
            AliCaptchaHelper.SecretProvider = aliSecretProvider;

        CaptchaHelper.DefaultValidator = SingleInstance<CaptchaValidator>.Instance;
        return services;
    }
}