using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OSS.Clients.SMS.HW;
using OSS.Common;
using OSS.Tools.Config;

namespace OSS.Core.Module.Notify
{
    public class NotifyServiceStarter : AppStarter
    {
        public override void Start(IServiceCollection serviceCollection)
        {
            InsContainer<INotifyCommonService>.Set<NotifyService>();
            InsContainer<ITemplateCommonService>.Set<TemplateService>();

            // 华为短信配置
            HwSmsHelper.default_secret = new HwSecret();
            ConfigHelper.Configuration.GetSection("Client:HuaWeiYun_Sms").Bind(HwSmsHelper.default_secret);

            // 邮件配置信息
            EmailHelper.default_smtp_config = new EmailSmtpConfig();
            ConfigHelper.Configuration.GetSection("Client:Email").Bind(EmailHelper.default_smtp_config);
        }
    }
}
