using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Common.Resp;
using OSS.Tools.DirConfig;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Services.Plugs.Notify.Mos;
using OSS.Core.Services.Plugs.Notify.NotifyAdapters.EmailHandlers.Mos;

namespace OSS.Core.Services.Plugs.Notify
{
    /// <summary>
    ///  获取
    /// </summary>
    public partial class NotifyService
    {
        #region 账号配置

        #region 短信

        #endregion

        #region 邮箱

        /// <summary>
        ///   获取邮箱账号配置 
        /// </summary>
        /// <returns></returns>
        public async Task<Resp<EmailSmtpConfig>> GetEmailConfig()
        {
            var config = await DirConfigHelper.GetDirConfig<EmailSmtpConfig>(CoreDirConfigKeys.plugs_notify_email_defult);
            if (config == null)
            {
                return new Resp<EmailSmtpConfig>().WithResp(RespTypes.OperateObjectNull, "未发现配置信息");
            }
            return new Resp<EmailSmtpConfig>(config);
        }

        /// <summary>
        /// 设置邮箱账号配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<Resp> SetEmailConfig(EmailSmtpConfig config)
        {
            var res = await DirConfigHelper.SetDirConfig(CoreDirConfigKeys.plugs_notify_email_defult, config);
            return res ? new Resp() : new Resp(RespTypes.OperateFailed, "设置华为云短信账号信息失败！");
        }


        #endregion

        #endregion

        #region 设置默认模板

        private static readonly Dictionary<string, string> _tempalteDirs;
        static NotifyService()
        {
            _tempalteDirs = new Dictionary<string, string>
            {
                { CoreDirConfigKeys.plugs_notify_email_log_tcode, "邮件日志模板" },
                { CoreDirConfigKeys.plugs_notify_email_portal_tcode, "邮件登录验证码模板" },
                { CoreDirConfigKeys.plugs_notify_sms_portal_tcode, "短信登录验证码模板" }
            };
        }

        /// <summary>
        ///  获取配置模板code字典
        /// </summary>
        /// <returns></returns>
        public Resp<Dictionary<string, string>> GetTemplateDirs()
        {
            return new Resp<Dictionary<string, string>>(_tempalteDirs);
        }

        /// <summary>
        ///   获取通知模板 
        /// </summary>
        /// <returns></returns>
        public async Task<Resp<NotifyTemplateConfig>> GetTemplateConfig(string templateCode)
        {
            string dirKey = string.Concat(CoreDirConfigKeys.plugs_notify_template_bycode, templateCode);

            var config = await DirConfigHelper.GetDirConfig<NotifyTemplateConfig>(dirKey);
            if (config == null)
            {
                return new Resp<NotifyTemplateConfig>().WithResp(RespTypes.OperateObjectNull, "未发现配置信息");
            }
            return new Resp<NotifyTemplateConfig>(config);
        }


        /// <summary>
        /// 设置通知模板信息
        /// </summary>
        /// <returns></returns>
        public async Task<Resp> SetTemplateConfig(string templateCode, NotifyTemplateConfig config)
        {
            string dirKey = string.Concat(CoreDirConfigKeys.plugs_notify_template_bycode, templateCode);
            var res = await DirConfigHelper.SetDirConfig(dirKey, config);
            return res ? new Resp() : new Resp(RespTypes.OperateFailed, "设置华为云短信账号信息失败！");
        }

        #endregion

    }
}
