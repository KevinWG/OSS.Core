using OSS.Common;
using OSS.Core.Domain;

namespace OSS.Core.Module.AppCenter
{
    public class AccessInfoMo : BaseOwnerMo<long> , IAccessSecret
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AccessInfoMo()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="name"></param>
        /// <param name="accessKey"></param>
        /// <param name="accessecret"></param>
        public AccessInfoMo(AppPlatform platform, string name, string accessKey, string accessecret)
        {
            this.name     = name;
            this.platform = platform;

            access_secret = accessecret;
            access_key    = accessKey;
        }


        /// <inheritdoc/>
        public string access_key { get; }
        /// <inheritdoc/>
        public string access_secret { get; }
        
        /// <summary>
        ///  应用名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        ///  平台信息
        /// </summary>
        public AppPlatform platform { get; set; }



        /// <summary>
        ///  扩展信息
        /// </summary>
        public string? ext { get; set; }

    }
}
