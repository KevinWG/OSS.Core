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

using System;
using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Extension;
using OSS.Core.RepDapper.Basic.Portal.Mos;
using OSS.Core.RepDapper.Basic.SocialPlats.Mos;

namespace OSS.Core.RepDapper.Basic.Portal
{
    public class OauthUserRep:BaseRep<OauthUserRep,OauthUserMo>
    {
        protected override string GetTableName()
        {
            return "p_portal_oauthusers";
        }
        /// <summary>
        ///  根据第三方用户编号获取本地授权信息
        /// </summary>
        /// <param name="appUId"></param>
        /// <param name="plat"></param>
        /// <returns></returns>
        public async Task<Resp<OauthUserMo>> GetOauthUserByAppUserId(string appUId, SocialPlatform plat)
        {
            return await Get(u => u.app_user_id == appUId
                                  && u.social_plat == plat);
        }


        /// <summary>
        ///   更新已有授权信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Resp> UpdateUserWithToken(OauthUserMo user)
        {
            return await Update(
                u => new
                {
                    u.app_union_id,
                    u.head_img,
                    u.nick_name,
                    u.sex,
                    u.access_token,
                    u.expire_date,
                    u.refresh_token,
                    u.status
                },
                w => w.id == user.id , user);
        }



        //public override Task<Resp<OauthUserMo>> GetById(string id)
        //{
        //    return base.GetById(id);
        //}

        /// <summary>
        ///   更新第三方用户信息的绑定用户id
        /// </summary>
        /// <param name="oauthUserId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Resp> BindUserIdByOauthId(long oauthUserId, long userId)
        {
            var mTime = DateTime.Now.ToUtcSeconds();
            return await Update(u => new { u_id = userId, status=UserStatus.Normal,m_time= mTime },
                w => w.id == oauthUserId );
        }
    }
}
