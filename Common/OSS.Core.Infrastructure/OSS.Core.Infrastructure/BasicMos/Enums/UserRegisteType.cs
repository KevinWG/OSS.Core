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

namespace OSS.Core.Infrastructure.BasicMos.Enums
{
    /// <summary>
    ///     注册类型
    /// </summary>
    public enum RegLoginType
    {
        /// <summary>
        ///     手机号
        /// </summary>
        Mobile = 20,

        /// <summary>
        ///     邮箱
        /// </summary>
        Email = 30,

        /// <summary>
        ///  第三方授权注册(昵称)
        /// </summary>
        ThirdName =40,
    }
}