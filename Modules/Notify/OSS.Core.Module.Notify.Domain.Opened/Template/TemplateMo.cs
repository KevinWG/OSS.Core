
using OSS.Core.Domain;

namespace OSS.Core.Module.Notify
{
    public class TemplateMo:BaseOwnerAndStateMo<long>
    {
        /// <summary>
        ///  推送类型
        /// </summary>
        public NotifyType notify_type { get; set; }

        /// <summary>
        ///  推送通道
        /// </summary>
        public NotifyChannel notify_channel { get; set; }


        /// <summary>
        ///  发送人或者发送通道号码等
        /// </summary>
        public string channel_sender { get; set; } = string.Empty;

        /// <summary>
        ///  模板外部平台编号
        /// </summary>
        public string channel_template_code { get; set; } = string.Empty;

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; } = string.Empty;

        /// <summary>
        ///  模板内容
        /// </summary>
        public string content { get; set; } = string.Empty;

        /// <summary>
        ///  是否html
        /// </summary>
        public ushort is_html { get; set; }

        /// <summary>
        ///  签名
        /// </summary>
        public string sign_name { get; set; } = string.Empty;
    }

}
