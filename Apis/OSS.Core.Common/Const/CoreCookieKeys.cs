#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore ——  系统关键Key
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

namespace OSS.Core.Infrastructure.Const
{
    /// <summary>
    ///     系统 Cookie Key
    /// </summary>
    public static class CoreCookieKeys
    {
        #region  cookie等名称

        /// <summary>
        ///    接口访问头信息验证票据名称
        /// </summary>
        public const string AuthorizeTicketName = "at-id";
        
        /// <summary>
        ///     用户验证cookie名称
        /// </summary>
        public const string UserCookieName = "uc_n";

        #endregion
    }
}