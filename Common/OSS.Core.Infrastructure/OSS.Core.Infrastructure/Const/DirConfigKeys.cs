namespace OSS.Core.Infrastructure.Const
{
    public static class DirConfigKeys
    {
        #region 通知模块(Notify)

        public const string plugs_notify_email_defult = "plugs_notify_email_defult";



        #region 通知模块-模板配置及相关code

        /// <summary>
        ///  通知模板配置键值  （+ code
        /// </summary>
        public const string plugs_notify_template_bycode = "plug_notify_template_";

        /// <summary>
        ///  通知模块- 邮件日志模板code
        /// </summary>
        public const string plugs_notify_email_log_tcode = "email_log";

        /// <summary>
        ///  通知模块- 邮件验证码登录模板code
        /// </summary>
        public const string plugs_notify_email_portal_tcode = "email_portal";


        /// <summary>
        ///  通知模块- 短信验证码登录模板code
        /// </summary>
        public const string plugs_notify_sms_portal_tcode = "sms_portal";
        #endregion

        #endregion
    }
}
