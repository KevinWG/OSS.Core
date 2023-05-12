namespace OSS.Core.Context
{
    /// <summary>
    ///  租户认证信息
    /// </summary>
    public class TenantIdentity
    {
        /// <summary>
        ///  租户Id
        /// </summary>
        public string id { get; set; } = string.Empty;

        /// <summary>
        /// 租户名称
        /// </summary>
        public string name { get; set; } = string.Empty;

        /// <summary>
        ///  logo 图像
        /// </summary>
        public string logo { get; set; } = string.Empty;
    }
}
