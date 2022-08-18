#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 用户注册登录类型
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

namespace OSS.Core.Module.Portal
{
  
    /// <summary>
    ///  平台登录应用类型
    /// </summary>
    public enum PortalType
    {
        /// <summary>
        ///  系统账号密码
        /// </summary>
        Password = 0,

        /// <summary>
        ///   动态验证码
        /// </summary>
        Code = 1,

        /// <summary>
        ///  微信公众号
        /// </summary>
        Wehcat_Official = 10010,

        /// <summary>
        ///  小程序
        /// </summary>
        Wechat_MApp = 10020,
    }

    /// <summary>
    ///  名称类型（手机号，邮箱）
    /// </summary>
    [Flags]
    public enum PortalNameType
    {
        /// <summary>
        ///     手机号
        /// </summary>
        Mobile = 1,

        /// <summary>
        ///     邮箱
        /// </summary>
        Email = 2,
    }
    
}