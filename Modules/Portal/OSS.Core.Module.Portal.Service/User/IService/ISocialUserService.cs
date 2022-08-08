using OSS.Common.Resp;

namespace OSS.Core.Module.Portal
{
    internal interface ISocialUserService
    {
        /// <summary>
        ///  新增或者更新社交平台用户授权信息
        /// </summary>
        /// <param name="socialUser"></param>
        /// <returns></returns>
        Task<LongResp> AddOrUpdateSocialUser(AddOrUpdateSocialReq socialUser);
        
        /// <summary>
        ///  绑定系统用户
        /// </summary>
        /// <param name="socialUserId"></param>
        /// <param name="sysUserId"></param>
        /// <returns></returns>
        Task<IResp<SocialUserMo>> BindWithSysUser(long socialUserId, long sysUserId);

        /// <summary>
        ///  获取社交平台用户信息
        /// </summary>
        /// <param name="socialUserId"></param>
        /// <returns></returns>
        Task<IResp<SocialUserMo>> GetById(long socialUserId);
    }
}
