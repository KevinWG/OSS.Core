#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore仓储层 —— 授权用户仓储
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-10-24
*       
*****************************************************************************/

#endregion

using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal
{
    /// <summary>
    ///  第三方用户仓储
    /// </summary>
    public class SocialUserRep : BasePortalRep< SocialUserMo>,ISocialUserRep
    {
        public SocialUserRep() : base("b_portal_social_user")
        {
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PageList<SocialUserSmallMo>> Search(SearchReq req)
        {
            return SimpleSearch<SocialUserSmallMo>(req);
        }


        /// <summary>
        ///  根据绑定用户id获取第三方账号信息
        /// </summary>
        /// <returns></returns>
        public  Task<IResp<SocialUserMo>> GetByUserId(long uId,string socialAppId, AppPlatform plat)
        {
            return  Get(u => u.owner_uid == uId && u.social_app_key == socialAppId && u.social_plat == plat);
        }

        /// <summary>
        ///  根据第三方用户编号获取本地授权信息
        /// </summary>
        /// <param name="appUserId"></param>
        /// <param name="plat"></param>
        /// <returns></returns>
        public  Task<IResp<SocialUserMo>> GetByAppUserId(string appUserId, string socialAppId, AppPlatform plat)
        {
            return  Get(u => u.app_user_id == appUserId && u.social_app_key == socialAppId && u.social_plat == plat);
        }



        /// <summary>
        ///   更新已有授权信息
        /// </summary>
        /// <returns></returns>
        public async Task<IResp> UpdateSocialUser(long socialUserId, AddOrUpdateSocialReq req)
        {
            return await Update(
                u => new
                {
                    u.app_union_id,
                    u.head_img,
                    u.nick_name,
                    u.sex,

                    u.access_token,
                    u.refresh_token,
                    u.expire_date,

                    u.ext
                },
                w => w.id == socialUserId, req);
        }

        /// <summary>
        ///   更新第三方用户信息的绑定用户id
        /// </summary>
        /// <param name="socialUserId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IResp> BindUserIdById(long socialUserId, long userId)
        {
            var mTime = DateTime.Now.ToUtcSeconds();
            return await Update(u => new { owner_uid = userId, status = UserStatus.Normal },
                w => w.id == socialUserId);
        }

        /// <summary>
        ///  更新微信小程序授权登录的会话信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appUnionId"></param>
        /// <returns></returns>
        public async Task<IResp> UpdateWechatMAppSession(long id, string sessionKey, string appUnionId)
        {
            var mTime = DateTime.Now.ToUtcSeconds();
            return await Update(u => new { access_token = sessionKey, app_union_id = appUnionId }, w => w.id == id);
        }
    }
}
