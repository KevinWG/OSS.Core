using OSS.Clients.SMS.HW;
using OSS.Tools.Config;

namespace OSS.Core.Module.Notify;

/// <summary>
///  通知模块涉及秘钥配置等处理
/// </summary>
public class NotifyAccessConfigStarter : AppStarter
{
    public override void Start(IServiceCollection serviceCollection)
    {
        // 华为短信配置
        HwSmsHelper.default_secret = new HwSecret();
        ConfigHelper.Configuration.GetSection("Access:SMS:HuaWeiYun").Bind(HwSmsHelper.default_secret);

        // 邮件配置信息
        EmailHelper.default_smtp_config = new EmailSmtpConfig();
        ConfigHelper.Configuration.GetSection("Access:Email").Bind(EmailHelper.default_smtp_config);
    }
}
