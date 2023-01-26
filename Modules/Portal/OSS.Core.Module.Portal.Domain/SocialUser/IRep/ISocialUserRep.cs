using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal
{
    public interface ISocialUserRep:IRepository<SocialUserMo,long>
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PageList<SocialUserSmallMo>> Search(SearchReq req);

        /// <summary>
        ///  根据绑定用户id获取第三方账号信息
        /// </summary>
        /// <returns></returns>
        Task<Resp<SocialUserMo>> GetByUserId(long uId, string socialAppId, AppPlatform plat);

        /// <summary>
        ///  根据第三方用户编号获取本地授权信息
        /// </summary>
        /// <returns></returns>
        Task<Resp<SocialUserMo>> GetByAppUserId(string appUserId, string socialAppKey, AppPlatform plat);

        /// <summary>
        ///   更新已有授权信息
        /// </summary>
        /// <returns></returns>
        Task<Resp> UpdateSocialUser(long socialUserId, AddOrUpdateSocialReq req);

        /// <summary>
        ///   更新第三方用户信息的绑定用户id
        /// </summary>
        /// <param name="socialUserId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Resp> BindUserIdById(long socialUserId, long userId);

        /// <summary>
        ///  更新微信小程序授权登录的会话信息
        /// </summary>
        /// <returns></returns>
        Task<Resp> UpdateWechatMAppSession(long id, string sessionKey, string appUnionId);
    }
}
