using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Portal;

/// <summary>
///  权限接口
/// </summary>
public class RoleController : BasePortalController, IOpenedRoleService
{
    private static readonly IOpenedRoleService _service = new RoleService();
    
    #region 角色处理

    /// <summary>
    ///  搜索角色
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_role_list)]
    public Task<PageListResp<RoleMo>> Search([FromBody] SearchReq req)
    {
        return _service.Search(req);
    }

    /// <summary>
    ///   删除角色
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_role_add)]
    public Task<IResp> Add([FromBody] AddRoleReq req)
    {
        return _service.Add(req);
    }

    /// <summary>
    ///  更新角色
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_role_add)]
    public Task<IResp> UpdateName(long id, [FromBody] AddRoleReq req)
    {
        return _service.UpdateName(id,req);
    }

    /// <summary>
    ///   启用角色
    /// </summary>
    /// <param name="rid"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_role_active)]
    public Task<IResp> Active(long rid)
    {
        return _service.Active(rid);
    }

    /// <summary>
    ///  作废角色
    /// </summary>
    /// <param name="rid"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_role_active)]
    public Task<IResp> UnActive(long rid)
    {
        return _service.UnActive(rid);
    }

    #endregion


    #region 角色用户绑定管理

    /// <summary>
    /// 获取用户角色信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [UserFuncMeta(PortalConst.FuncCodes.portal_user_roles)]
    public Task<ListResp<RoleMo>> GetUserRoles(long userId)
    {
        return _service.GetUserRoles(userId);
    }
    

    /// <summary>
    ///   添加新的绑定
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_role_bind_user)]
    public Task<IResp> UserBind([FromBody] AddRoleUserReq req)
    {
        return _service.UserBind(req);
    }

    /// <summary>
    ///  删除用户角色绑定关系
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_role_bind_delete)]
    public Task<IResp> DeleteUserBind(long userId,long roleId)
    {
        return _service.DeleteUserBind(userId,roleId);
    }

    ///// <summary>
    /////  搜索用户角色绑定关系
    ///// </summary>
    ///// <param name="req">
    /////filter:
    ///// r_name => 角色名称
    ///// u_name => 用户名称    
    ///// </param>
    ///// <returns></returns>
    //[HttpPost]
    //[UserFuncMeta(PortalConst.FuncCodes.Permit_RoleUserSearch)]
    //public Task<PageListResp<RoleUserBigMo>> SearchRoleUsers([FromBody] SearchReq req)
    //{
    //    return _service.SearchRoleUsers(req);
    //}


    #endregion
}