using System.ComponentModel.DataAnnotations;
using OSS.Common.ComModels;
using OSS.Core.Infrastructure.Enums;

namespace OSS.Core.WebSite.Controllers.Users.Mos
{

    #region  手机号登录注册（包含手机动态验证码
    
    /// <summary>
    ///   手机号动态码注册登录 请求
    /// </summary>
    public class UserMobileCodeRegLoginReq
    {
        /// <summary>
        /// 手机号验证码
        /// </summary>
        [Required(ErrorMessage = "验证码必填"), MinLength(4, ErrorMessage = "请填写正确的验证码！")]
        public string pass_code { get; set; }

        /// <summary>
        ///  手机号
        /// </summary>
        [Required, DataType(DataType.PhoneNumber, ErrorMessage = "请输入正确的手机号")]
        public string mobile { get; set; }
    }

    #endregion

    /// <summary>
    /// 正常用户注册登录请求实体
    /// </summary>
    public class UserRegLoginReq
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
        [Required(ErrorMessage = "密码必填"), MinLength(6, ErrorMessage = "密码不得少于6位")]
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
