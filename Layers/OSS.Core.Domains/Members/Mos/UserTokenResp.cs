using OSS.Common.ComModels;

namespace OSS.Core.Domains.Members.Mos
{
    public class UserTokenResp:ResultMo
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfoMo user { get; set; }

        /// <summary>
        ///  用户Token信息
        /// </summary>
        public string token { get; set; }
    }
}
