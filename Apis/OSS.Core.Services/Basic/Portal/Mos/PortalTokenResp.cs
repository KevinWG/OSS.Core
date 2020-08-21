using OSS.Common.BasicMos.Resp;
using OSS.Core.Context.Mos;
using OSS.Core.RepDapper.Basic.Portal.Mos;

namespace OSS.Core.Services.Basic.Portal.Mos
{
 
    public class PortalTokenResp:Resp<UserIdentity>
    {
        public PortalTokenResp()
        {
        }

        public PortalTokenResp(RespTypes result, string message)
        {
            ret = (int)result;
            msg = message;
        }

        /// <summary>
        ///  用户Token信息
        /// </summary>
        public string token { get; set; }
    }



    public class SocialRegisterConfig
    {
        /// <summary>
        ///  授权用户注册类型
        /// </summary>
        public OauthRegisterType OauthRegisterType { get; set; }
    }

}
