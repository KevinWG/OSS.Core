using System;
using System.Linq;
using System.Threading.Tasks;
using OSS.Clients.SMS.Ali;
using OSS.Clients.SMS.Ali.Reqs;
using OSS.Common.BasicImpls;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Services.Plugs.Notify.EmailImp;
using OSS.Core.Services.Plugs.Notify.EmailImp.Mos;
using OSS.Core.Services.Plugs.Notify.IProxies;
using OSS.Core.Services.Plugs.Notify.Mos;
using OSS.Tools.Log;

namespace OSS.Core.Services.Plugs.Notify
{
    public partial class NotifyService : INotifyServiceProxy
    {
        /// <summary>
        ///     发送通知消息
        /// </summary>
        /// <param name="msg">消息实体</param>
        /// <param name="isFromLogModule">【重要】是否来自日志模块，直接决定当前方法内部异常写日志的操作</param>
        /// <returns></returns>
        public async Task<NotifyResp> Send(NotifyReq msg,bool isFromLogModule=false)
        {
            string errCode = null;
            try
            {
                var tRes = GetMsgTemplate(msg.t_code);
                if (!tRes.IsSuccess())
                    return new NotifyResp().WithResp(tRes);

                //  todo 添加发送记录
                return await NotifyMsg(tRes.data,msg, isFromLogModule);
            }
            catch (Exception ex)
            {
                if (!isFromLogModule)
                {
                    errCode = LogHelper.Error($"发送信息出错,错误信息：{ex.Message}", this.GetType().Name);
                }
            }

            return new NotifyResp().WithResp(SysRespTypes.ApplicationError, $"发送消息失败,错误码：{errCode}！");
        }


        #region 获取Notify实例及对应配置

        private  Task< NotifyResp> NotifyMsg(NotifyTemplateMo template, NotifyReq msg, bool isFromLogModule)
        {
            switch (template.notify_type)
            {
                case NotifyType.Email:
                    return NotifyEmailMsg(template, msg, isFromLogModule);
                case NotifyType.SMS:
                    return NotifySmsMsg(template, msg, isFromLogModule);
            }

            return Task.FromResult(new NotifyResp().WithResp(RespTypes.UnKnowOperate, "未知通知消息类型！"));
        }


        #region 邮件的发送处理

        private async Task<NotifyResp> NotifyEmailMsg(NotifyTemplateMo template, NotifyReq msg, bool isFromLogModule)
        {
            var emailConfigRes = await GetEmailConfig(isFromLogModule);
            if (!emailConfigRes.IsSuccess())
                return new NotifyResp().WithResp(emailConfigRes);

            var body = msg.body_paras.Aggregate(template.content,
                (current, p) => current.Replace(string.Concat("{", p.Key, "}"), p.Value));

            var emailMsg = new EmailMsgMo
            {
                body      = body,
                is_html   = template.is_html,
                subject   = string.IsNullOrEmpty(msg.msg_title) ? template.title : msg.msg_title,
                to_emails = msg.targets
            };

            var eRes       = await EmailHelper.SendAsync(emailConfigRes.data, emailMsg);
            var notifyResp = new NotifyResp().WithResp(eRes);

            notifyResp.msg_biz_id = msg.msg_Id;

            return notifyResp;
        }

        #endregion


        private async Task<NotifyResp> NotifySmsMsg(NotifyTemplateMo template, NotifyReq msg,
            bool isFromLogModule)
        {
            //todo 添加不同通道处理
            switch (template.notify_channel)
            {
                case NotifyChannel.AliYun:
                    return await NotifySmsMsg_AliYun(template, msg, isFromLogModule);
            }
            return new NotifyResp().WithResp(RespTypes.UnKnowOperate, "未知短信发送通道");
        }

        private async Task<NotifyResp> NotifySmsMsg_AliYun(NotifyTemplateMo template, NotifyReq msg, bool isFromLogModule)
        {
         
            var aliSmsConfigRes = await GetAliSmsConfig(isFromLogModule);
            if (!aliSmsConfigRes.IsSuccess())
             return new NotifyResp().WithResp(aliSmsConfigRes);

            var smsClient = SingleInstance<AliSmsClient>.Instance;
            smsClient.SetContextConfig(aliSmsConfigRes.data);

            var sendMsg = GetSendAliSmsReq(template, msg);

            return (await smsClient.Send(sendMsg)).ToNotifyResp();
        }


        private static SendAliSmsReq GetSendAliSmsReq(NotifyTemplateMo template, NotifyReq msg)
        {
            var aliMsg = new SendAliSmsReq();

            aliMsg.PhoneNums = msg.targets;
            aliMsg.body_paras = msg.body_paras;
            aliMsg.sign_name = template.sign_name;
            aliMsg.template_code = template.t_plat_code;

            return aliMsg;
        }


        #endregion
    }
}
