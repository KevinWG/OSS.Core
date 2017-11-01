#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 对OSS.Core.WebApi 缓存Key辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-6-5
*       
*****************************************************************************/

#endregion

namespace OSS.Core.WebSite.AppCodes.Tools
{
    public class CacheKeysUtil
    {
        /// <summary>
        ///  当前登录用户信息缓存键
        /// </summary>
        public const string CurrentUserInfo = "current_user_info";

        /// <summary>
        /// 当前登录用户缓存小时数
        ///  这个缓存使用的是绝对过期时间
        /// </summary>
        public const int CurrentUserInfoHours = 2;
    }
}
