namespace OSS.Core.Module.Portal;

/// <summary>
///  授权相关配置信息
/// </summary>
public class AuthSetting
{
    /// <summary>
    /// 登录注册时短信模板Id
    /// </summary>
    public string? SmsTemplateId { get; set; }

    /// <summary>
    /// 登录注册时邮箱模板Id
    /// </summary>
    public string? EmailTemplateId { get; set; }

}