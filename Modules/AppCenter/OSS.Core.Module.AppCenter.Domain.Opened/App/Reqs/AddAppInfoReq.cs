using OSS.Core.Domain;

namespace OSS.Core.Module.AppCenter.Domain
{
    public class AddAppInfoReq
    {
        /// <summary>
        ///  外部平台Id
        /// </summary>
        public string? outer_app_id { get; set; }
        
        /// <summary>
        ///  平台信息
        /// </summary>
        public AppPlatform platform { get; set; }

        /// <summary>
        ///  应用对应秘钥
        /// </summary>
        public string app_secret { get; set; }

        /// <summary>
        ///  应用名称
        /// </summary>
        public string name { get; set; }
    }
}
