using OSS.Common.Resp;
using System.Net.Mail;

namespace OSS.Core.Module.Notify
{
    public class EmailHandler : INotifyHandler
    {
        public async Task<NotifySendResp> NotifyMsg(TemplateMo template, NotifySendReq msg)
        {
            var body = msg.body_paras == null
                ? template.content
                : msg.body_paras.Aggregate(template.content,
                    (current, p) => current.Replace(string.Concat("{", p.Key, "}"), p.Value));

            var mm = new MailMessage
            {
                IsBodyHtml = template.is_html > 0,
                Priority   = MailPriority.Normal,
                Subject    = string.IsNullOrEmpty(msg.msg_title) ? template.title : msg.msg_title,
                Body       = body
            };

            foreach (var mailItem in msg.targets)
            {
                mm.To.Add(new MailAddress(mailItem));
            }

            var eRes       = await EmailSmtpClient.SendAsync(mm);
            var notifyResp = new NotifySendResp().WithResp(eRes);

            notifyResp.msg_biz_id = msg.msg_Id;

            return notifyResp;
        }

    }
}
