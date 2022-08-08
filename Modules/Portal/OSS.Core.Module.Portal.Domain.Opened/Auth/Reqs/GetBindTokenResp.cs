using OSS.Common.Resp;

namespace OSS.Core.Module.Portal
{
    public class GetBindTokenResp : Resp
    {
        /// <summary>
        ///   绑定信息的令牌
        /// </summary>
        public string token { get; set; }
    }
}
