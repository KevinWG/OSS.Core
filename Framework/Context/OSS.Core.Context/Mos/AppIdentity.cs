﻿#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：通用系统授权信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*****************************************************************************/

#endregion

namespace OSS.Core.Context
{
    /// <summary>
    ///   来源（请求方）应用的授权认证信息
    /// </summary>
    public class AppIdentity : AppAuthReq
    {
        /// <summary>
        ///  应用授权信息构造函数
        /// </summary>
        public AppIdentity()
        {
            ask_meta  = new AskMeta();
        }

        //  ==========  通过请求附带信息确定的属性

        /// <summary>
        ///  当前请求主机信息 
        /// </summary>
        public string host { get; set; } = string.Empty;

        /// <summary>
        /// 来源模式
        /// </summary> 
        public AppAuthMode auth_mode { get; set; }

        /// <summary>
        ///   应用类型
        /// </summary>
        public AppType type { get; set; } = AppType.Normal;

        /// <summary>
        /// 用户授权信息
        /// </summary>
        public string authorization { get; set; } = string.Empty;

        /// <summary>
        ///  针对当前请求相关的预定义要求信息
        /// </summary>
        public AskMeta ask_meta { get; set; }
    }

    /// <summary>
    ///     请求相关的预定义要求信息
    /// </summary>
    public class AskMeta
    {
        /// <summary>
        /// 要求授权模式
        /// </summary> 
        public AppAuthMode app_auth_mode { get; set; } = AppAuthMode.None;

        /// <summary>
        ///  要求的应用类型
        /// </summary>
        public AppType app_type { get; set; } = AppType.Normal;

        /// <summary>
        ///  要求的租户类型
        /// </summary>
        public TenantType tenant_type { get; set; } = TenantType.Normal;

        /// <summary>
        ///  请求模块名称
        ///     （系统定义
        /// </summary>
        public string module_name { get; set; } = string.Empty;

        /// <summary>
        ///  要求的权限码
        /// </summary>
        public string func_code { get; set; } = string.Empty;

        /// <summary>
        ///  要求的用户身份类型 
        /// </summary>
        public UserIdentityType user_identity_type { get; set; } = UserIdentityType.NormalUser;
    }


    /// <summary>
    ///  应用授权执行模式（值越小安全层级越高
    /// </summary>
    public enum AppAuthMode
    {
        /// <summary>
        ///  系统应用签名模式（强签名）
        /// </summary>
        AppSign = 0,

        /// <summary>
        ///  第三方合作应用约定模式（如微信回调，自定义约定验证模式
        /// </summary>
        PartnerContract = 1000,

        /// <summary>
        ///  非授权验证
        /// </summary>
        None = 10000
    }
}