using System.Data;
using Microsoft.Extensions.Configuration;
using OSS.Common;
using OSS.Tools.Config;

namespace OSS.Core.Extension.Mvc.Captcha;

public static class AliCaptchaHelper
{
    /// <summary>
    ///  阿里验证码客户端应用秘钥提供者
    /// 默认读取：AliYun:Captcha  节点
    /// </summary>
    public static IAccessProvider<AliCaptchaSecret>? SecretProvider { get; set; } =
        SingleInstance<AliCaptchaSecretDefaultProvider>.Instance;
}

public class AliCaptchaSecret : AccessSecret
{
    /// <summary>
    ///  应用key
    /// </summary>
    public string app_key { get; set; } = string.Empty;
}

/// <summary>
///   阿里云的验证码秘钥提供者
/// </summary>
internal class AliCaptchaSecretDefaultProvider : IAccessProvider<AliCaptchaSecret>
{
    public Task<AliCaptchaSecret> Get()
    {
        var secret = new AliCaptchaSecret();
        ConfigHelper.Configuration.GetSection("Captcha:AliYun").Bind(secret);

        if (string.IsNullOrEmpty(secret.access_secret))
            throw new NoNullAllowedException("未能找到阿里云验证码访问配置信息!");

        return Task.FromResult(secret);
    }
}