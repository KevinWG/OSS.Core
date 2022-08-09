using OSS.Common.Resp;
using OSS.Core.Context;

namespace OSS.Core.Module.Portal
{
    public class PortalTokenResp : Resp<UserIdentity>
    {
        public PortalTokenResp()
        {
        }

        public PortalTokenResp(RespCodes result, string message)
        {
            code = (int)result;
            msg = message;
        }

        /// <summary>
        ///  用户Token信息
        /// </summary>
        public string token { get; set; } = string.Empty;
    }


    public class SocialRegisterConfig
    {
        /// <summary>
        ///  授权用户注册类型
        /// </summary>
        public SocialRegisterType reg_type { get; set; }
    }

}
