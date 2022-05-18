
namespace OSS.Core.Services.Basic.Portal.Reqs
{
    public class BindUserPortalReq : PortalPasscodeReq
    {
        /// <summary>
        ///  绑定令牌
        /// </summary>
        public string bind_token { get; set; }
    }
}
