using OSS.Core.Infrastructure.Enums;

namespace OSS.Core.Domains.Members.Mos
{
    public class ThirdPlatformUserMo
    {
        /// <summary>
        /// 性别
        /// </summary>
        public Sex sex { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string nick_names { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public string expire_date { get; set; }
    }
}
