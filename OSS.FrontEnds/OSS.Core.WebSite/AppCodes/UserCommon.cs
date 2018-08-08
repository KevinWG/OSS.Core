#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 用户信息公用类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-23
*       
*****************************************************************************/

#endregion

using System;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Plugs.CachePlug;
using OSS.Core.Infrastructure.Utils;
using OSS.Core.WebSite.AppCodes.Tools;
using OSS.Core.WebSite.Controllers.Users.Mos;

namespace OSS.Core.WebSite.AppCodes
{
    /// <summary>
    ///   用户通用辅助类
    /// </summary>
    public static class UserCommon
    {
        public static async Task<ResultMo<UserInfoMo>> GetCurrentUser()
        {
            var user = CacheUtil.Get<UserInfoMo>(CacheKeysUtil.CurrentUserInfo);
            if (user != null)
            {
                return new ResultMo<UserInfoMo>(user);
            }

            var userRes = await RestApiUtil.PostCoreApi<ResultMo<UserInfoMo>>("/member/GetCurrentUser",);
            if (!userRes.IsSuccess())
                return userRes.ConvertToResultOnly<UserInfoMo>();

            CacheUtil.AddOrUpdate(CacheKeysUtil.CurrentUserInfo, userRes.data, TimeSpan.Zero,
                DateTime.Now.AddHours(CacheKeysUtil.CurrentUserInfoHours));

            return userRes;
        }
    }
}
