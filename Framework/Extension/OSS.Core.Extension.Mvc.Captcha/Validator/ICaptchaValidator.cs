using Microsoft.AspNetCore.Http;
using OSS.Common.Resp;

namespace OSS.Core.Extension.Mvc.Captcha;

/// <summary>
///  验证码验证器
/// </summary>
public interface ICaptchaValidator
{
    /// <summary>
    ///  验证码验证方法
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task<Resp> Validate(HttpContext context);
}