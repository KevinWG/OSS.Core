namespace OSS.Core.Module.Portal
{
    public class BindByPassCodeReq : PortalPasscodeReq
    {
        /// <summary>
        ///  绑定令牌
        /// </summary>
        public string bind_token { get; set; } = string.Empty;
    }
}
