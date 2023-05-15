using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  签名秘钥提供者
    /// </summary>
    public interface IAppAccessProvider
    {
        /// <summary>
        ///  通过Key值获取应用签名信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<Resp<AppAccess>> GetByKey(string key);
    }

    /// <summary>
    /// 签名秘钥信息
    /// </summary>
    public class AppAccess : IAccessSecret
    {
        /// <summary>
        /// 秘钥key
        /// </summary>
        public string access_key { get; set; }

        /// <summary>
        ///  秘钥密匙
        /// </summary>
        public string access_secret { get; set; }
    }
}
