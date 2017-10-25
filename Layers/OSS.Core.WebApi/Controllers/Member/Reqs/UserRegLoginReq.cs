#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 用户注册请求实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

using System.ComponentModel.DataAnnotations;
using OSS.Core.Infrastructure.Enums;

namespace OSS.Core.WebApi.Controllers.Member.Reqs
{
    public class UserLoginBaseReq
    {
        /// <summary>
        /// 登录注册类型
        ///  没有使用Range验证，拆装箱效率太低
        /// </summary>
        public RegLoginType type { get; set; }

        /// <summary>
        /// 手机号 或者 邮箱
        /// </summary>
        [Required(ErrorMessage = "请填写账号!")]
        public string name { get; set; }
    }

    /// <summary>
    /// 正常用户注册登录请求实体
    /// </summary>
    public class UserPasswordReq: UserLoginBaseReq
    {
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "请填写密码!"), MinLength(6, ErrorMessage = "密码不能少于六位！")]
        public string password { get; set; }

    }

    public class UserPasscodeReq: UserLoginBaseReq
    {
        /// <summary>
        /// 动态验证码
        /// </summary>
        [Required(ErrorMessage = "请填写验证密码!"), MinLength(4, ErrorMessage = "验证码不少于四位!")]
        public string passcode { get; set; }
    }


}
