using System.ComponentModel.DataAnnotations;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.BasicMos.Enums;


namespace OSS.CorePro.TAdminSite.Apis.Portal.Reqs
{
    public class LoginNameReq
    {
        /// <summary>
        /// 登录注册类型
        /// </summary>
        public RegLoginType type { get; set; }

        /// <summary>
        /// 手机号 或者 邮箱
        /// </summary>
        [Required(ErrorMessage = "登录账号不能为空！")]
        public string name { get; set; }
    }

    /// <summary>
    /// 正常用户注册登录请求实体
    /// </summary>
    public class CodeLoginReq: LoginNameReq
    {
        /// <summary>
        ///  手机号注册时需要验证码
        /// </summary>
        [Required(ErrorMessage = "验证码不能为空！")]
        public string code { get; set; }
    }


    public class PwdLoginReq : LoginNameReq
    {
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空！")]
        public string password { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// 用户注册登录后响应实体
    /// </summary>
    public class UserRegLoginResp:Resp<UserIdentity>
    {
        /// <summary>
        /// 用户token，作为用户授权判断依据
        /// </summary> 
        public string token { get; set; }
    }
}
