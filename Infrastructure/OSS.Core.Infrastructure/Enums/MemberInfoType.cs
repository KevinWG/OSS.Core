#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 上下文成员信息类型
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

namespace OSS.Core.Infrastructure.Enums
{
    /// <summary>
    ///  成员信息类型枚举
    /// </summary>
    public enum MemberInfoType
    {
        OnlyId,
        Info
    }

    /// <summary>
    /// 成员授权类型
    /// </summary>
    public enum MemberAuthorizeType
    {
        /// <summary>
        ///  第三方临时授权用户
        /// </summary>
        OauthUserTemp,
        
        /// <summary>
        /// 系统用户
        /// </summary>
        User,

        /// <summary>
        ///  后台管理员
        /// </summary>
        Admin,

        /// <summary>
        ///  超级管理员
        /// </summary>
        SuperAdmin
    }
    
    /// <summary>
    ///  授权注册时绑定类型
    ///   可以根据平台自定义
    /// </summary>
    public enum OauthRegisteType
    {
        /// <summary>
        ///  直接注册
        /// </summary>
        JustRegiste,

        /// <summary>
        ///  选择是否绑定
        /// </summary>
        ChooseBind,

        /// <summary>
        /// 绑定
        /// </summary>
        Bind
    }
    
}
