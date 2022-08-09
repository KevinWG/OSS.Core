using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal;

public interface IRoleUserRep : IRepository<RoleUserMo, long>
{
    /// <summary>
    ///  获取当前授权用户的功能权限列表
    /// </summary>
    /// <returns></returns>
    Task<List<long>> GetRoleIdsByUserId(long userId);

    /// <summary>
    ///  通过角色Id获取用户数量
    ///  【主要在管理端用，暂无缓存】
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task<IResp<int>> GetUserCountByRoleId(long roleId);

    /// <summary>
    ///  删除绑定信息
    /// </summary>
    /// <returns></returns>
    Task<IResp> DeleteBind(long userId, long roleId);

    ///// <summary>
    /////  搜索用户角色绑定关联
    ///// </summary>
    ///// <param name="req"></param>
    ///// <returns></returns>
    //Task<PageList<RoleUserBigMo>> SearchRoleUsers(SearchReq req);
}