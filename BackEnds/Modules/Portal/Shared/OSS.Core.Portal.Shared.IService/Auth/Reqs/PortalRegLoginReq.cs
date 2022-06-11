#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 用户注册请求实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Portal.Domain;
using System.ComponentModel.DataAnnotations;

namespace OSS.Core.Portal.Shared.IService
{
    public class PortalNameReq
    {
        /// <summary>
        /// 登录注册类型
        /// </summary>
        public PortalType type { get; set; }

        /// <summary>
        /// 手机号 或者 邮箱
        /// </summary>
        [Required(ErrorMessage = "请填写账号信息!")]
        public string name { get; set; } =string.Empty;
    }

    /// <summary>
    ///  登录请求基类
    /// </summary>
    public class PortalLoginBaseReq : PortalNameReq
    {
        /// <summary>
        /// 是否来自第三方授权后绑定系统账号的登录注册请求
        ///     0-不是（默认）
        ///     1-是 （如微信Oauth注册
        /// </summary>
        public int is_social_bind { get; set; }
    }


    /// <summary>
    /// 正常用户注册登录请求实体
    /// </summary>
    public class PortalPasswordReq : PortalLoginBaseReq
    {
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "请填写密码!"), MinLength(6, ErrorMessage = "密码不能少于六位！")]
        public string? password { get; set; }
    }

    /// <summary>
    ///  验证码登录请求
    /// </summary>
    public class PortalPassCodeReq : PortalLoginBaseReq
    {
        /// <summary>
        /// 动态验证码
        /// </summary>
        [Required(ErrorMessage = "请填写验证密码!"), MinLength(4, ErrorMessage = "验证码不少于四位!")]
        public string? code { get; set; }
    }

    public static class PortalLoginBaseReqExtension
    {
        /// <summary>
        ///   检查验证登录类型
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static Resp CheckNameType(this PortalNameReq req)
        {
            if (string.IsNullOrEmpty(req.name))
                return new Resp(RespTypes.ParaError, "账号不能为空！");

            if (!Enum.IsDefined(req.type.GetType(), req.type))
                return new Resp(RespTypes.ParaError, "未知的账号类型！");

            var validator = new DataTypeAttribute(
                req.type == PortalType.Mobile
                    ? DataType.PhoneNumber
                    : DataType.EmailAddress);

            return !validator.IsValid(req.name)
                ? new Resp(RespTypes.ParaError, "请输入正确的手机或邮箱！")
                : new Resp();
        }
    }


    public class PortalTokenResp : Resp<UserIdentity>
    {
        public PortalTokenResp()
        {
        }

        public PortalTokenResp(RespTypes result, string message)
        {
            code = (int)result;
            msg = message;
        }

        /// <summary>
        ///  用户Token信息
        /// </summary>
        public string token { get; set; } = string.Empty;
    }
}
