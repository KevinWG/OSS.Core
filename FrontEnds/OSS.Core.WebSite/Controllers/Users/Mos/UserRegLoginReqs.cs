using System.ComponentModel.DataAnnotations;
using OSS.Common.ComModels;
using OSS.Core.Infrastructure.Enums;

namespace OSS.Core.WebSite.Controllers.Users.Mos
{
    /// <summary>
    /// 正常用户注册登录请求实体
    /// </summary>
    public class CodeLoginReq
    {
        /// <summary>
        /// 登录注册类型
        ///  没有使用Range验证，拆装箱效率太低
        /// </summary>
        public RegLoginType type { get; set; }

        /// <summary>
        /// 手机号 或者 邮箱
        /// </summary>
        [Required(ErrorMessage = "登录账号不能为空！")]
        public string name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string pass_word { get; set; }

        /// <summary>
        ///  手机号注册时需要验证码
        /// </summary>
        public string pass_code { get; set; }
    }
    
    /// <summary>
    /// 用户注册登录后响应实体
    /// </summary>
    public class UserRegLoginResp:ResultMo
    {
        /// <summary>
        /// 用户token，作为用户授权判断依据
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfoMo user { get; set; }

        /// <summary>
        ///  返回地址
        /// </summary>
        public string return_url { get; set; }
    }



}
