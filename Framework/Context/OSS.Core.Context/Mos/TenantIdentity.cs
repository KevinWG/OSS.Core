namespace OSS.Core.Context
{
    public class TenantIdentity
    {
        public string id { get; set; }

        /// <summary>
        /// 租户平台名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///  logo 图像
        /// </summary>
        public string logo { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //public long last_update_time { get; set; }

        ///// <summary>
        ///// 扩展信息
        ///// </summary>
        //public object TenantInfo { get; set; }
    }
}
