#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore ——  系统关键Key
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

namespace OSS.Core.Infrastructure.Utils
{
    /// <summary>
    /// 系统关键Key
    /// </summary>
    public class GlobalKeysUtil
    {
        #region  cookie等名称

        /// <summary>
        ///  验证票据名称
        /// </summary>
        public const string AuthorizeTicketName = "at_id";

        /// <summary>
        ///  用户验证cookie名称
        /// </summary>
        public const string UserCookieName = "ct_id";

        /// <summary>
        /// 管理员验证cookie名称
        /// </summary>
        public const string AdminCookieName = "cat_id";


        /// <summary>
        ///  用户登录注册成功后跳转来源地址的cookie名称
        /// </summary>
        public const string UserReturnUrlCookieName = "r_url";

        #endregion


    }
}
