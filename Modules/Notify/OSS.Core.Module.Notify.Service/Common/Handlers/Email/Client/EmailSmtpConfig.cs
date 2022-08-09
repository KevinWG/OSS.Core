using OSS.Common;

namespace OSS.Core.Module.Notify
{
    /// <summary>
    ///  邮件发送配置
    /// </summary>
    public class EmailSmtpConfig:IAccess
    {
        /// <summary>
        ///  主机信息
        /// </summary>
        public string host { get; set; } = string.Empty;
       
        /// <summary>
        /// 端口信息
        /// </summary>
        public int port { get; set; } 

        /// <summary>
        /// 是否开启ssl加密
        /// </summary>
        public int enable_ssl { get; set; } 

        /// <summary>
        ///  发送者的邮箱地址
        /// </summary>
        public string email { get; set; } = string.Empty;


        /// <summary>
        ///  发送者的邮箱名称
        /// </summary>
        public string email_name { get; set; } = string.Empty;

        /// <summary>
        ///  发送邮箱密码
        /// </summary>
        public string password { get; set; } = string.Empty;
    }
}
