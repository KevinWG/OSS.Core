using OSS.Common;

namespace OSS.Core.Module.Notify;

public class EmailHelper
{
    /// <summary>
    ///  邮件配置提供者
    /// </summary>
    public static IEmailSmtpConfigProvider? smtp_config_provider { get; set; } 
    public static void SetSmtpConfigprovider(IEmailSmtpConfigProvider config)
    {
        smtp_config_provider = config;
    }

    /// <summary>
    ///  邮件默认配置
    /// </summary>
    public static EmailSmtpConfig? default_smtp_config { get; set; }
}


public interface IEmailSmtpConfigProvider : IAccessProvider<EmailSmtpConfig>
{

}


