using System.ComponentModel.DataAnnotations;
using OSS.Common.ComModels;

namespace OSS.Core.WebSite.Controllers.Mos.Reqs
{

    #region  手机号登录注册（包含手机动态验证码
    /// <summary>
    ///  正常手机号注册
    /// </summary>
    public class UserMobileRegReq : UserMobileLoginReq
    {
        /// <summary>
        /// 手机号验证码
        /// </summary>
        [Required(ErrorMessage = "验证码必填"), MinLength(4,ErrorMessage = "请填写正确的验证码！")]
        public string pass_code { get; set; }
    }

    /// <summary>
    /// 手机号登录
    /// </summary>
    public class UserMobileLoginReq : UserMobileBaseReq
    {
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码必填"),MinLength(6,ErrorMessage = "密码不得少于6位")]
        public string pass_word { get; set; }
    }

    /// <summary>
    ///   手机号动态码注册登录 请求
    /// </summary>
    public class UserMobileCodeRegLoginReq:UserMobileBaseReq
    {
        /// <summary>
        /// 手机号验证码
        /// </summary>
        [Required(ErrorMessage = "验证码必填"), MinLength(4, ErrorMessage = "请填写正确的验证码！")]
        public string pass_code { get; set; }
    }

    public class UserMobileBaseReq
    {
        /// <summary>
        ///  手机号
        /// </summary>
        [Required, DataType(DataType.PhoneNumber,ErrorMessage = "请输入正确的手机号")]
        public string mobile { get; set; }
    }

    #endregion

    /// <summary>
    /// 用户邮箱注册登录请求实体
    /// </summary>
    public class UserRegLoginEmailReq
    {
        /// <summary>
        ///  手机号
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string pass_word { get; set; }
    }



    /// <summary>
    /// 用户注册登录后响应实体
    /// </summary>
    public class UserRepLoginResp:ResultMo
    {
        /// <summary>
        /// 用户token，作为用户授权判断依据
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfoMo user { get; set; }
    }



}
