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
using Newtonsoft.Json;
using OSS.Common.ComModels;
using OSS.Core.Domains.Members.Mos;
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
        [Required(ErrorMessage = "密码必填"), MinLength(6, ErrorMessage = "密码不得少于6位")]
        public string pass_word { get; set; }

        /// <summary>
        ///  手机号注册时需要验证码
        /// </summary>
        public string pass_code { get; set; }
    }


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
    }
}
