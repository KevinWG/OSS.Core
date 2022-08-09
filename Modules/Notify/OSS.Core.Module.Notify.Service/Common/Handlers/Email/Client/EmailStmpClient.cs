using OSS.Common.Resp;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace OSS.Core.Module.Notify
{
    public static class EmailSmtpClient
    {

        /// <summary>
        /// 发送服务邮件
        /// </summary>
        /// <param name="mMsg"></param>
        /// <returns></returns>
        public static async Task<IResp> SendAsync(MailMessage mMsg)
        {
            var emailConfig = await GetConfig();

            mMsg.From = new MailAddress(emailConfig.email, emailConfig.email_name, Encoding.UTF8);

            // todo  注意性能优化-client是否可以复用？
            var client = new SmtpClient()
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl      = emailConfig.enable_ssl > 0,
                Host           = emailConfig.host,
                Port           = emailConfig.port,
                Credentials    = new NetworkCredential(emailConfig.email, emailConfig.password)
            };

            using (client)
            {
                await client.SendMailAsync(mMsg);
                return new Resp();
            }
        }

        private static async Task<EmailSmtpConfig> GetConfig()
        {
            var config = EmailHelper.smtp_config_provider != null
                ? await EmailHelper.smtp_config_provider.Get()
                : EmailHelper.default_smtp_config;

            if (config == null)
            {
                throw new NoNullAllowedException("未能找到邮件发送的配置信息!");
            }

            return config;
        }
    }
}
