
namespace OSS.Core.Portal.Shared.IService
{
    public class BindUserPortalReq : PortalPassCodeReq
    {
        /// <summary>
        ///  绑定令牌
        /// </summary>
        public string bind_token { get; set; }
    }
}
