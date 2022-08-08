using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal;

public interface IUserInfoRep:IRepository<UserInfoMo,long>
{
    /// <summary>
    ///  获取平台列表
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    Task<PageList<UserBasicMo>> SearchUsers(SearchReq search);

    /// <summary>
    ///  获取用户信息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    Task<IResp<UserInfoMo>> GetUserByLoginType(string name, PortalNameType type);

    /// <summary>
    ///  修改用户登录信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<IResp> UpdatePortalByType(long id, PortalNameType type, string name);

    /// <summary>
    ///  修改用户状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    Task<IResp> UpdateStatus(long id, UserStatus state);

    /// <summary>
    ///  修改基础信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="avatar"></param>
    /// <param name="nickName"></param>
    /// <returns></returns>
    Task<IResp> UpdateBasicInfo(long id, string avatar, string nickName);
}