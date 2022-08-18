
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal
{
    public class UserBasicMo : BaseOwnerMo<long>
    {
        /// <summary>
        ///  昵称
        /// </summary>
        public string? nick_name { get; set; }

        /// <summary>
        ///  头像信息
        /// </summary>
        public string? avatar { get; set; }

        /// <summary>
        ///  邮件地址
        /// </summary>
        public string? email { get; set; }

        /// <summary> 
        ///  手机号
        /// </summary>
        public string? mobile { get; set; }

        /// <summary>
        ///  用户状态
        /// </summary>
        public UserStatus status { get; set; }
    }
}
