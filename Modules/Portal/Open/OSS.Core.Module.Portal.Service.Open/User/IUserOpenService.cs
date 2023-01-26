using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Portal;

public interface IUserOpenService
{
    /// <summary>
    ///  修改基础信息
    /// </summary>
    /// <returns></returns>
    Task<Resp> ChangeMyBasic(UpdateUserBasicReq req);

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
    Task<Resp<UserBasicMo>> Get(long userId);

    /// <summary>
    /// 锁定
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Resp> Lock(long id);

    /// <summary>
    ///  解锁
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Resp> UnLock(long id);
}