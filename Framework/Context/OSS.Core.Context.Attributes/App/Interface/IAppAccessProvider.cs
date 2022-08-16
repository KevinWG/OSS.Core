using OSS.Common;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  签名秘钥提供者
    /// </summary>
    public interface IAppSignAccessProvider
    {
        public Task<AppSignAccess>
    }

    /// <summary>
    /// 签名秘钥信息
    /// </summary>
    public class AppSignAccess : IAccessSecret
    {
        public string access_key { get; set; }

        public string access_secret { get; set; }

        /// <summary>
        ///   应用类型
        /// </summary>
        public AppType app_type { get; set; }
    }
}
