using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Core.Infrastructure.Enums;

namespace OSS.Core.Domains.Members.Mos
{
    public class UserRegisteConfig
    {
        /// <summary>
        ///  授权用户注册类型
        /// </summary>
        public OauthRegisteType OauthRegisteType { get; set; }

        /// <summary>
        ///  邮箱是否需要验证
        /// </summary>
        public bool EmailCheck { get; set; }
    }

    public class UserTokenResp:ResultMo
    {
        public UserTokenResp()
        {
            
        }

        public UserTokenResp(ResultTypes result, string message)
        {
            ret = (int)result;
            msg = message;
        }


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
