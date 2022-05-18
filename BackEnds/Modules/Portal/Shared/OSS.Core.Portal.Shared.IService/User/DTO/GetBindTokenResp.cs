using OSS.Common.Resp;

namespace OSS.Core.Services.Basic.Portal.Reqs
{
    public class GetBindTokenResp : Resp
    {
        /// <summary>
        ///   绑定信息的令牌
        /// </summary>
        public string token { get; set; }
    }
}
