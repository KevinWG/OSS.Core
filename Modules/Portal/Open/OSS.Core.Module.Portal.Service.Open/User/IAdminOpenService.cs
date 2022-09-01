using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Portal;

public interface IAdminOpenService
{
    /// <summary>
    ///   管理员修改自己头像地址
    /// </summary>
    /// <param name="avatar"></param>
    /// <returns></returns>
    Task<IResp> ChangeMyAvatar(string avatar);

    /// <summary>
    ///   管理员修改自己的名称
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<IResp> ChangeMyName(string name);

    /// <summary>
    ///  添加管理员
    /// </summary>
    /// <param name="admin"></param>
    /// <returns></returns>
    Task<Resp<long>> AddAdmin(AdminInfoMo admin);

    /// <summary>
    ///  管理员查询
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<PageListResp<AdminInfoMo>> SearchAdmins(SearchReq req);

    /// <summary>
    /// 修改锁定状态
    /// </summary>
    /// <param name="uId"></param>
    /// <param name="makeLock"></param>
    /// <returns></returns>
    Task<IResp> ChangeLockStatus(long uId, bool makeLock);

    /// <summary>
    /// 修改锁定状态
    /// </summary>
    /// <param name="uId"></param>
    /// <param name="adminType"></param>
    /// <returns></returns>
    Task<IResp> SetAdminType(long uId, AdminType adminType);


}