using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Portal;

public interface IOpenedPermitService
{
    /// <summary>
    ///  获取当前授权用户下所有角色对应的全部权限列表
    ///    （ 10 分钟左右缓存误差）
    /// </summary>
    /// <returns></returns>
    Task<ListResp<GrantedPermit>> GetCurrentUserPermits();
    
    /// <summary>
    ///  获取当前角色下权限项列表
    /// </summary>
    /// <returns></returns>
    Task<ListResp<GrantedPermit>> GetPermitsByRoleId(long roleId);

    /// <summary>
    ///   修改角色下的关联权限
    /// </summary>
    /// <returns></returns>
    Task<IResp> ChangeRolePermits(long rid, ChangeRolePermitReq req);




}