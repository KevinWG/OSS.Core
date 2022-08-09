using Microsoft.AspNetCore.Http;
using OSS.Common.Resp;

namespace OSS.Core.Extension.Mvc.Captcha;

/// <summary>
///  默认实现
/// </summary>
internal class DefaultNoneCaptchaValidator: ICaptchaValidator
{
    private static readonly Task<IResp> _taskResp = Task.FromResult(Resp.DefaultSuccess);

    /// <inheritdoc />
    public Task<IResp> Validate(HttpContext context)
    {
        return _taskResp;
    }
}