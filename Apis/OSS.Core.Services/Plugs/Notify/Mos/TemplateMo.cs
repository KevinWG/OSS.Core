namespace OSS.Adapters.Notify.Reqs
{
    public class TemplateMo
    {
        /// <summary>
        ///  模板外部平台编号
        /// </summary>
        public string t_plat_code { get; set; }
        
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
        public bool is_html { get; set; }

        /// <summary>
        ///  签名
        /// </summary>
        public string sign_name { get; set; }
    }
}
