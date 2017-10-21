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
        public string pass_word { get; set; }

        /// <summary>
        /// 动态验证码
        ///  登录时如果验证码不为空，则使用动态验证码登录
        /// </summary>
        public string pass_code { get; set; }
    }
}
