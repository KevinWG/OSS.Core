using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Portal.Domain;
using OSS.Core.Portal.Shared.IService;
using OSS.Core.Services.Basic.Portal.Reqs;

namespace OSS.Core.Portal.Service;

public interface IUserService : ISharedUserService
{
    /// <summary>
    ///  直接添加用户（管理员权限
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<Resp<long>> AddUser(UserInfoMo user);

    /// <summary>
    ///  修改基础信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<Resp> ChangeMyBasic(UpdateUserBasicReq req);

    /// <summary>
    ///  获取租户平台下的用户列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<PageListResp<UserInfoMo>> SearchUsers(SearchReq req);

    /// <summary>
    /// 获取我的用户信息
    /// </summary>
    /// <returns></returns>
    Task<Resp<UserBasicMo>> GetMyInfo();
    
    /// <summary>
    ///  修改锁定状态
    /// </summary>
    /// <param name="uId"></param>
    /// <param name="makeLock"></param>
    /// <returns></returns>
    Task<Resp> ChangeLockStatus(long uId, bool makeLock);

}