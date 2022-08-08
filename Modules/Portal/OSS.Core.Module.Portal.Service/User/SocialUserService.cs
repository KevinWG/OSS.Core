using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal
{
    public class SocialUserService:ISocialUserService
    {
        private static readonly ISocialUserRep _socialUserRep = InsContainer<ISocialUserRep>.Instance;

        /// <summary>
        ///  获取当前租户平台下的外部平台用户列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PageListResp<SocialUserSmallMo>> SearchSocialUsers(SearchReq req)
        {
            return new PageListResp<SocialUserSmallMo>(await _socialUserRep.Search(req));
        }
        
        /// <inheritdoc />
        public async Task<LongResp> AddOrUpdateSocialUser(AddOrUpdateSocialReq req)
        {
            var socialUserRes = await GetByAppUserId(req.app_user_id, req.social_app_key, req.social_plat);
            if (!socialUserRes.IsSysOk())
                return new LongResp().WithResp(socialUserRes);

            var oldSocialUser = socialUserRes.data;

            if (socialUserRes.IsRespCode(RespCodes.OperateObjectNull))
            {
                var newSocialUser = req.ToSocialUser();

                await _socialUserRep.Add(newSocialUser);

                return new LongResp(newSocialUser.id);
            }

            var updateRes = await _socialUserRep.UpdateSocialUser(oldSocialUser.id, req);
            
            return updateRes.IsSuccess()
                ? new LongResp(oldSocialUser.id)
                : new LongResp().WithResp(updateRes);
        }
        
        private Task<IResp<SocialUserMo>> GetByAppUserId(string appUserId, string socialAppKey, AppPlatform plat)
        {
            return _socialUserRep.GetByAppUserId(appUserId, socialAppKey, plat);
        }
        
        /// <inheritdoc />
        public async Task<IResp<SocialUserMo>> BindWithSysUser(long socialUserId, long sysUserId)
        {
            var socialUserRes = await _socialUserRep.GetById(socialUserId);
            if (!socialUserRes.IsSuccess())
                return new Resp<SocialUserMo>().WithResp(socialUserRes, "第三方账号信息异常！");

            var bindRes = await _socialUserRep.BindUserIdById(socialUserId, sysUserId);
            if (!bindRes.IsSuccess())
                return new Resp<SocialUserMo>().WithResp(bindRes, "绑定系统账号异常！");

            socialUserRes.data.owner_uid = sysUserId;

            return new Resp<SocialUserMo>(socialUserRes.data);
        }
        
        /// <inheritdoc/>
        public Task<IResp<SocialUserMo>> GetById(long socialUserId)
        {
            return _socialUserRep.GetById(socialUserId);
        }
    }

}
