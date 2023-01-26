﻿using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Portal;

public interface IRoleOpenService
{
    /// <summary>
    ///   搜索角色
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<PageListResp<RoleMo>> Search(SearchReq req);

    /// <summary>
    ///  添加角色
    /// </summary>
    /// <returns></returns>
    Task<Resp> Add(AddRoleReq req);

    /// <summary>
    ///  更新角色名称
    /// </summary>
    /// <returns></returns>
    Task<Resp> UpdateName(long id, AddRoleReq req);

    /// <summary>
    ///   激活角色
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Resp> Active(long id);

    /// <summary>
    ///  下线角色
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Resp> UnActive(long id);





    /// <summary>
    ///  获取当前授权用户的角色列表
    /// </summary>
    /// <returns></returns>
    Task<ListResp<RoleMo>> GetUserRoles(long userId);

    /// <summary>
    ///   添加新的绑定
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    Task<Resp> UserBind(AddRoleUserReq req);

    /// <summary>
    ///   删除角色绑定
    /// </summary>
    /// <returns></returns>
    Task<Resp> DeleteUserBind(long userId, long roleId);
}