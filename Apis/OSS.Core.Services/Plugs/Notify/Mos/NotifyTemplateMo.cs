namespace OSS.Core.Services.Plugs.Notify.Mos
{
    public class NotifyTemplateConfig
    {
        /// <summary>
        ///  模板编号
        /// </summary>
        public string t_code { get; set; }

        /// <summary>
        ///  模板外部平台编号
        /// </summary>
        public string t_plat_code { get; set; }

        /// <summary>
        ///  推送类型
        /// </summary>
        public NotifyType notify_type { get; set; }

        /// <summary>
        ///  推送通道
        /// </summary>
        public NotifyChannel notify_channel { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        ///  模板内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        ///  是否html
        /// </summary>
        public int is_html { get; set; }

        /// <summary>
        ///  签名
        /// </summary>
        public string sign_name { get; set; }

        /// <summary>
        ///  发送人或者发送通道号码等
        /// </summary>
        public string sender { get; set; }
    }


    //public static class NotifyTemplateMaps
    //{
    //    public static TemplateMo ConvertToAdapterTemplate(this NotifyTemplateMo nTemplate)
    //    {
    //        var t = new TemplateMo
    //        {
    //            title       = nTemplate.title,
    //            content     = nTemplate.content,
    //            is_html     = nTemplate.is_html,

    //            t_plat_code = nTemplate.t_plat_code,
    //            sign_name   = nTemplate.sign_name
    //        };
    //        return t;
    //    }
    //}

}
