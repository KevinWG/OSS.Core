using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal;

public interface IRoleRep : IRepository<RoleMo, long>
{
    /// <summary>
    ///  搜索角色
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<PageList<RoleMo>> SearchRoles(SearchReq req);

    /// <summary>
    ///  通过角色Ids获取角色列表
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<List<RoleMo>> GetList(IList<long> ids);

    /// <summary>
    /// 修改角色状态
    /// </summary>
    /// <param name="rid"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    Task<IResp> UpdateStatus(long rid, CommonStatus status);

    /// <summary>
    /// 修改角色名称
    /// </summary>
    /// <param name="rid"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<IResp> UpdateName(long rid, string name);
}