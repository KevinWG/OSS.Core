using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Clients.SMS.Ali.Reqs;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Helpers;
using OSS.Core.Services.Plugs.Config.IProxies;
using OSS.Core.Services.Plugs.Notify.EmailImp.Mos;
using OSS.Core.Services.Plugs.Notify.Mos;

namespace OSS.Core.Services.Plugs.Notify
{
    /// <summary>
    ///  获取
    /// </summary>
    public partial class NotifyService
    {
        private static Resp<NotifyTemplateMo> GetMsgTemplate(string tCode)
        {
            return _templates.TryGetValue(tCode, out NotifyTemplateMo template)
                ? new Resp<NotifyTemplateMo>(template)
                : new Resp<NotifyTemplateMo>().WithResp(RespTypes.ObjectNull, "没有对应模板");
        }

        #region 设置默认模板

        //  todo 后续修改为数据库配置，界面管理
        private static readonly Dictionary<string, NotifyTemplateMo> _templates =
            new Dictionary<string, NotifyTemplateMo>();

        private static void RegisterTemplate(NotifyTemplateMo template)
        {
            _templates.Add(template.t_code, template);
        }

        static NotifyService()
        {
            RegisterTemplate(new NotifyTemplateMo()
            {
                content =
                    " 系统 {module_name}  模块日志 <br/>    日志等级：{level}<br/>    日志key：{msg_key} <br/>    日志唯一编码：{log_id}    日志详情：{msg_body}<br/>",
                is_html = true,
                title = "系统日志",
                sign_name = "【系统】",

                t_code = "Email_Log_NotifyDetail",
                notify_type = NotifyType.Email
            });


            RegisterTemplate(new NotifyTemplateMo()
            {
                content = "你当前的动态码为：{code},如果不是你本人的操作,请忽略本条信息！",
                is_html = true,
                title = "动态验证码服务（注册/登录）",
                sign_name = "[淘梦科技]",

                t_code = "Email_Portal_LoginCode",
                notify_type = NotifyType.Email
            });

            RegisterTemplate(new NotifyTemplateMo()
            {
                sign_name = "千贝云",
                t_code = "SMS_Portal_LoginCode",
                t_plat_code = "SMS_126865409",
                notify_type = NotifyType.SMS,
                notify_channel = NotifyChannel.AliYun
            });
        }

        #endregion

        #region 相关账号设置
        
        private const string _aliSmsConfigKey = "plugs_notify_sms_ali";
        public Task<Resp<AliSmsConfig>> GetAliSmsConfig(bool isFromLogModule = false)
        {
            // 可扩展租户平台处理
            return InsContainer<IDirConfigServiceProxy>.Instance.GetConfig<AliSmsConfig>(_aliSmsConfigKey, isFromLogModule);
        }

        // todo  后续可以配置和模板code关联
        private const string _emailConfigKey = "plugs_notify_email";
        public  Task<Resp<EmailSmtpConfig>>  GetEmailConfig(bool isFromLogModule = false)
        { 
            // 可扩展租户平台处理
           return InsContainer<IDirConfigServiceProxy>.Instance.GetConfig<EmailSmtpConfig>(_emailConfigKey, isFromLogModule);
        }


        #endregion
    }
}
