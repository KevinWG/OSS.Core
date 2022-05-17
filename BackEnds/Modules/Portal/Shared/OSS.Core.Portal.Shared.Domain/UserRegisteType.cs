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


namespace OSS.Core.Portal.Shared.Domain
{
    /// <summary>
    ///     账号类型
    /// </summary>
    public enum PortalType
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
        ThirdOauth = 40,

        /// <summary>
        ///  第三方平台账号关注
        /// </summary>
        ThirdFollow = 50,
    }



    /// <summary>
    ///  可发送code类型
    /// </summary>
    public enum PortalCodeType
    {
        /// <summary>
        ///     手机号
        /// </summary>
        Mobile = 20,

        /// <summary>
        ///     邮箱
        /// </summary>
        Email = 30,
    }

    public static class PortalCodeTypeMap
    {
        /// <summary>
        ///  可发送code账号类型 转 账号类型
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static PortalType ToPortalType(this PortalCodeType code)
        {
            switch (code)
            {
                case PortalCodeType.Mobile:
                    return PortalType.Mobile;
                default:
                    return PortalType.Email;
            }
        }
    }


}