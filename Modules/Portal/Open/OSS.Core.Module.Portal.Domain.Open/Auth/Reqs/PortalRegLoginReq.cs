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
using System.ComponentModel.DataAnnotations;

namespace OSS.Core.Module.Portal
{
    /// <summary>
    ///  登录请求基类
    /// </summary>
    public class BaseRegLoginReq : PortalNameReq
    {
        /// <summary>
        /// 是否来自第三方授权后绑定系统账号的登录注册请求
        ///     0-不是（默认）
        ///     1-是 （如微信Oauth注册
        /// </summary>
        public int is_social_bind { get; set; }

        /// <summary>
        ///  记住自动登录
        /// </summary>
        public bool remember { get; set; }
    }
    
    /// <summary>
    /// 正常用户注册登录请求实体
    /// </summary>
    public class PortalPasswordReq : BaseRegLoginReq
    {
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "请填写密码!"), MinLength(6, ErrorMessage = "密码不能少于六位！")]
        public string password { get; set; } = string.Empty;
    }

    /// <summary>
    ///  验证码登录请求
    /// </summary>
    public class PortalPasscodeReq : BaseRegLoginReq
    {
        /// <summary>
        /// 动态验证码
        /// </summary>
        [Required(ErrorMessage = "请填写验证密码!"), MinLength(4, ErrorMessage = "验证码不少于四位!")]
        public string code { get; set; }
    }

    public static class PortalLoginBaseReqExtension
    {
        /// <summary>
        ///   检查验证登录类型
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static IResp CheckNameType(this PortalNameReq req)
        {
            if (string.IsNullOrEmpty(req.name))
                return new Resp(RespCodes.ParaError, "账号不能为空！");

            if (!Enum.IsDefined(req.type.GetType(), req.type))
                return new Resp(RespCodes.ParaError, "未知的账号类型！");

            var validator = new DataTypeAttribute(
                req.type == PortalNameType.Mobile
                    ? DataType.PhoneNumber
                    : DataType.EmailAddress);

            return !validator.IsValid(req.name)
                ? new Resp(RespCodes.ParaError, "请输入正确的账号信息！")
                : new Resp();
        }
    }
}
