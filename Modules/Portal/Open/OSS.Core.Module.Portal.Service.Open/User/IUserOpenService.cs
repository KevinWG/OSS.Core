using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Portal;

public interface IUserOpenService
{
    /// <summary>
    ///  修改基础信息
    /// </summary>
    /// <returns></returns>
    Task<IResp> ChangeMyBasic(UpdateUserBasicReq req);

    /// <summary>
    ///  获取租户平台下的用户列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<PageListResp<UserBasicMo>> SearchUsers(SearchReq req);

    /// <summary>
    ///   获取用户信息（管理员权限
    /// </summary>
    /// <returns></returns>
    Task<Resp<UserBasicMo>> GetUserById(long userId);

    /// <summary>
    ///  修改锁定状态
    /// </summary>
    /// <param name="uId"></param>
    /// <param name="makeLock"></param>
    /// <returns></returns>
    Task<IResp> ChangeLockStatus(long uId, bool makeLock);

}