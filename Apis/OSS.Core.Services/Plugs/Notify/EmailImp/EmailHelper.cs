#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore插件 —— 邮件的插件实现
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-10-22
*       
*****************************************************************************/

#endregion

using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Services.Plugs.Notify.EmailImp.Mos;

namespace OSS.Core.Services.Plugs.Notify.EmailImp
{
    /// <summary>
    ///  邮件帮助类
    /// </summary>
    public class EmailHelper
    {
        /// <summary>
        /// 发送服务邮件
        /// </summary>
        /// <param name="emailConfig"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static async Task<Resp> SendAsync(EmailSmtpConfig emailConfig, EmailMsgMo msg)
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
                EnableSsl = emailConfig.enable_ssl,
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