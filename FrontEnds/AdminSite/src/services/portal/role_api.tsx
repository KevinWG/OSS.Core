import { request } from '@umijs/max';

export async function searchRoles(searchReq: SearchReq) {
  return request<IPageListResp<PortalApi.RoleMo>>('/portal/role/Search', {
    method: 'POST',
    data: searchReq,
  });
}

export function addRole(req: { name: string }) {
  return request<IResp>('/portal/Role/Add', {
    method: 'POST',
    data: req,
  });
}

export function updateRoleName(id: string, req: { name: string }) {
  return request<IResp>('/portal/Role/UpdateName?id=' + id, {
    method: 'POST',
    data: req,
  });
}

export function activeRole(id: string) {
  return request<IResp>('/portal/role/Active?rid=' + id, {
    method: 'POST',
  });
}

export function unactiveRole(id: string) {
  return request<IResp>('/portal/role/UnActive?rid=' + id, {
    method: 'POST',
  });
}

/**
 * 获取角色对应权限项
 */
export async function getRoleFuncs(r_id: string) {
  return request<IListResp<PortalApi.RoleFuncItem>>('/portal/Grant/GetPermitsByRoleId?rid=' + r_id);
}

/**
 * 修改角色对应权限信息
 */
export async function editRoleFuncs(r_id: string, add_items: string[], delete_items: string[]) {
  return request<IResp>('/portal/Grant/ChangeRolePermits?rid=' + r_id, {
    method: 'POST',
    data: { add_items, delete_items },
  });
}

/**
 * 获取用户角色信息
 * @param id 用户id
 * @returns
 */
export function getUserRoles(id: string) {
  return request<IListResp<PortalApi.RoleMo>>('/portal/Role/GetUserRoles?userId=' + id);
}

/**
 *  删除用户角色绑定
 */
export async function deleteUserRoleBind(userId: string, roleId: string): Promise<IResp> {
  return request<IResp>('/portal/role/DeleteUserBind?userId=' + userId + '&roleId=' + roleId, {
    method: 'POST',
  });
}

/**
 * 添加用户角色绑定信息
 */
export async function addUserRoleBind(rid: string, uid: string) {
  return request<IResp>('/portal/role/UserBind', {
    method: 'POST',
    data: {
      role_id: rid,
      u_id: uid,
    },
  });
}
