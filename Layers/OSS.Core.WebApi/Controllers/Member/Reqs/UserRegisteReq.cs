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
using OSS.Common.ComModels;
using OSS.Core.DomainMos.Members.Mos;
using OSS.Core.Infrastructure.Enums;

namespace OSS.Core.WebApi.Controllers.Member.Reqs
{
    public class UserRegisteReq
    {

        /// <summary>
        ///注册类型，邮箱或者手机号等
        /// </summary>
        public RegLoginType reg_type { get; set; }

        /// <summary>
        ///  手机号
        /// </summary>
        [Required]
        public string value { get; set; }

        /// <summary>
        ///  密码或者动态登录时的手机验证码
        /// </summary>
        [Required]
        public string pass_code { get; set; }

    }


    public class UserRegisteResp:ResultMo
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
