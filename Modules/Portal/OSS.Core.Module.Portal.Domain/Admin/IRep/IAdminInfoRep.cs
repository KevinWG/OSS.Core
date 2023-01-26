using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal;

public interface IAdminInfoRep : IRepository<AdminInfoMo, long>
{
    /// <summary>
    ///   通过用户id获取管理员信息
    /// </summary>
    /// <param name="uId"></param>
    /// <returns></returns>
    Task<Resp<AdminInfoMo>> GetAdminByUId(long uId);

    /// <summary>
    ///  查询管理员列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<PageList<AdminInfoMo>> SearchAdmins(SearchReq req);

    /// <summary>
    ///  修改管理员状态
    /// </summary>
    /// <param name="uId"></param>
    /// <param name="adminStatus"></param>
    /// <returns></returns>
    Task<Resp> UpdateStatus(long uId, AdminStatus adminStatus);

    /// <summary>
    ///   修改头像地址
    /// </summary>
    /// <param name="uId"></param>
    /// <param name="avatar"></param>
    /// <returns></returns>
    Task<Resp> ChangeAvatar(long uId, string avatar);

    /// <summary>
    /// 修改管理员名称
    /// </summary>
    /// <param name="uId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<Resp> ChangeMyName(long uId, string name);

    /// <summary>
    ///  修改管理员状态
    /// </summary>
    /// <param name="uId"></param>
    /// <param name="adminType"></param>
    /// <returns></returns>
    Task<Resp> SetAdminType(long uId, AdminType adminType);
}