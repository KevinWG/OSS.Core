﻿using Microsoft.AspNetCore.Http;
using OSS.Common.Resp;

namespace OSS.Core.Extension.Mvc.Captcha;

/// <summary>
///  默认实现
/// </summary>
internal class DefaultNoneCaptchaValidator: ICaptchaValidator
{
    /// <inheritdoc />
    public Task<Resp> Validate(HttpContext context)
    {
        return Task.FromResult(Resp.Success());
    }
}