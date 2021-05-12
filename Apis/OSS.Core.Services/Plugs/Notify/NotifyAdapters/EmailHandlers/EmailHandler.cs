
using OSS.Common.BasicMos.Resp;
using OSS.Tools.DirConfig;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Services.Plugs.Notify.Mos;
using OSS.Core.Services.Plugs.Notify.NotifyAdapters.EmailHandlers.Mos;
using OSS.Core.Services.Plugs.Notify.SmsAdapters.Handlers;

namespace OSS.Core.Services.Plugs.Notify.NotifyAdapters.EmailHandlers
{
    public class EmailHandler : INotifyHandler
    {
        public Task<EmailSmtpConfig> GetEmailConfig()
        {
            // 可扩展租户平台处理
            return DirConfigHelper.GetDirConfig<EmailSmtpConfig>(DirConfigKeys.plugs_notify_email_defult);
        }

        public async Task<NotifyResp> NotifyMsg(NotifyTemplateConfig template, NotifyReq msg)
        {
            var config = await GetEmailConfig();
            if (config==null)
                return new NotifyResp().WithResp(RespTypes.ObjectNull,"未发现邮件配置信息！");

            var body = msg.body_paras.Aggregate(template.content,
                (current, p) => current.Replace(string.Concat("{", p.Key, "}"), p.Value));

            var emailMsg = new EmailMsgMo
            {
                body = body,
                is_html = template.is_html>0,
                subject = string.IsNullOrEmpty(msg.msg_title) ? template.title : msg.msg_title,
                to_emails = msg.targets
            };

            var eRes = await SendAsync(config, emailMsg);
            var notifyResp = new NotifyResp().WithResp(eRes);

            notifyResp.msg_biz_id = msg.msg_Id;

            return notifyResp;
        }




        /// <summary>
        /// 发送服务邮件
        /// </summary>
        /// <param name="emailConfig"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static async Task<Resp> SendAsync(EmailSmtpConfig emailConfig, EmailMsgMo msg)
        {
            var mm = new MailMessage
            {
                IsBodyHtml = true,
                Priority = MailPriority.Normal,
                From = new MailAddress(emailConfig.email, emailConfig.email_name, Encoding.UTF8),
                Subject = msg.subject,
                Body = msg.body
            };


            foreach (var mailItem in msg.to_emails)
            {
                mm.To.Add(new MailAddress(mailItem));
            }
            var client = new SmtpClient()
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = emailConfig.enable_ssl>0,
                Host = emailConfig.host,
                Port = emailConfig.port,
                Credentials = new NetworkCredential(emailConfig.email, emailConfig.password)
            };
            using (client)
            {
                await client.SendMailAsync(mm);
                return new Resp();
            }
        }
    }
}
