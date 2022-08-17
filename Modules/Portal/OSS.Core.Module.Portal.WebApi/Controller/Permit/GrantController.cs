using Microsoft.AspNetCore.Mvc;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Portal;

/// <summary>
///  权限接口
/// </summary>
public class GrantController : BasePortalController, IOpenedPermitService
{
    private static readonly IOpenedPermitService _service = new PermitService();

    /// <summary>
    ///  获取当前登录用户的权限列表
    ///      （可能出现 10 分钟左右缓存误差）
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<ListResp<GrantedPermit>> GetCurrentUserPermits()
    {
        return _service.GetCurrentUserPermits();
    }

    /// <summary>
    ///  判断登录用户是否具有某权限
    /// </summary>
    /// <param name="funcCode"></param>
    /// <param name="sceneCode"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IResp<FuncDataLevel>> CheckPermit(string funcCode, string sceneCode)
    {
        return _service.CheckPermit(funcCode, sceneCode);
    }


    #region 角色和功能项关联处理

    /// <summary>
    ///  获取当前角色下权限项列表
    /// </summary>
    /// <returns></returns> 
    [HttpGet]
    [UserFuncMeta(PortalConst.FuncCodes.portal_grant_role_permits)]
    public Task<ListResp<GrantedPermit>> GetPermitsByRoleId(long rid)
    {
        return _service.GetPermitsByRoleId(rid);
    }


    /// <summary>
    /// 获取所有权限列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_grant_role_change)]
    public Task<IResp> ChangeRolePermits(long rid, [FromBody] ChangeRolePermitReq items)
    {
        return _service.ChangeRolePermits(rid, items);
    }

    #endregion
}