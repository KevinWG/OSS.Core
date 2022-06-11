using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Portal.Domain
{
    public interface IUserInfoRep:IRepository<UserInfoMo,long>
    {
        /// <summary>
        ///  获取平台列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<PageListResp<UserInfoMo>> SearchUsers(SearchReq search);

        /// <summary>
        ///  获取用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<Resp<UserInfoMo>> GetUserByLoginType(string name, PortalType type);

        /// <summary>
        ///  修改用户登录信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Resp> UpdatePortalByType(long id, PortalCodeType type, string name);

        /// <summary>
        ///  修改用户状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        Task<Resp> UpdateStatus(long id, UserStatus state);

        /// <summary>
        ///  修改基础信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="avatar"></param>
        /// <param name="nickName"></param>
        /// <returns></returns>
        Task<Resp> UpdateBasicInfo(long id, string avatar, string nickName);
    }
}
