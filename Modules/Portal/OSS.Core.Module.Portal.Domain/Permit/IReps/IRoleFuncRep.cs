using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal;

public interface IRoleFuncRep : IRepository<RoleFuncMo, long>
{
    /// <summary>
    ///  获取指定角色的权限列表
    /// </summary>
    /// <returns></returns>
    Task<List<GrantedPermit>> GetRoleFuncList(long roleId);

    /// <summary>
    ///  获取多个角色的权限列表
    /// </summary>
    /// <returns></returns> 
    Task<List<GrantedPermit>> GetRoleFuncList(List<long> roleIds);

    /// <summary>
    ///  修改关联的权限项
    /// </summary>
    /// <param name="rid"></param>
    /// <param name="addItems"></param>
    /// <param name="deleteItems"></param>
    /// <returns></returns>
    Task<IResp> ChangeRoleFuncItems(long rid, List<RoleFuncMo>? addItems, List<string>? deleteItems);
}