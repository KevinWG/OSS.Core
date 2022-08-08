using Microsoft.AspNetCore.Http;

namespace OSS.Core.Extension.Mvc.Captcha;

/// <summary>
///  验证码验证器
/// </summary>
public interface ICaptchaValidatorProvider
{
    /// <summary>
    ///  验证码验证方法
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    ICaptchaValidator GetValidator(HttpContext context);
}