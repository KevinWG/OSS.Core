import { request } from 'umi';
import { SearchReq, PageListResp, Resp, ListResp } from '@/utils/resp_d';
import { RoleInfo, FuncItem, RoleFuncItem, RoleUserInfo } from './data_d';
import { AdminInfo } from '../portal/admin/data_d';

export async function searchRoles(searchReq: SearchReq) {
  return request<PageListResp<RoleInfo>>('/api/b/permit/SearchRoles', {
    method: 'POST',
    data: searchReq,
  });
}

export async function OperateRoleStatus(operate: string, record: RoleInfo): Promise<Resp> {
  switch (operate) {
    case 'active':
      return request<Resp>('/api/b/permit/RoleActive?rid=' + record.id, {
        method: 'POST',
      });
      break;
    case 'active_off':
      return request<Resp>('/api/b/permit/RoleUnActive?rid=' + record.id, {
        method: 'POST',
      });
      break;
    case 'delete':
      return request<Resp>('/api/b/permit/RoleDelete?rid=' + record.id, {
        method: 'POST',
      });
      break;
  }
  return new Promise<Resp>((resolve, reject) => {
    resolve({ ret: 0, is_failed: false, is_ok: true, msg: '' });
  });
}

export async function roleEdit(newFormVals: any, oldData?: RoleInfo) {
  if (!oldData) {
    return request<Resp>('/api/b/permit/roleadd', {
      method: 'POST',
      data: newFormVals,
    });
  }

  return request<Resp>('/api/b/permit/RoleUpdate', {
    method: 'POST',
    data: { id: oldData.id, ...newFormVals },
  });
}

/**
 * 获取所有权限项
 */
export async function getSysAllFuncs() {
  return request<ListResp<FuncItem>>('/api/b/permit/getAllFuncItems');
}

/**
 * 获取角色对应权限项
 */
export async function getRoleFuncs(r_id: string) {
  return request<ListResp<RoleFuncItem>>('/api/b/permit/GetRoleFuncList?rid=' + r_id);
}

/**
 * 修改角色对应权限信息
 */
export async function editRoleFuncs(r_id: string, add_items: string[], delete_items: string[]) {
  return request<ListResp<RoleFuncItem>>('/api/b/permit/ChangeRoleFuncItems?rid=' + r_id, {
    method: 'POST',
    data: { add_items, delete_items },
  });
}

/**
 * 搜索角色和管理员绑定信息
 */
export async function searchRoleUsers(searchReq: SearchReq) {
  return request<PageListResp<RoleUserInfo>>('/api/b/permit/SearchRoleUsers', {
    method: 'POST',
    data: searchReq,
  });
}

/**
 * 用户角色相关状态操作
 * todo 和服务端测试，直接放data里能不能通过方法参数接收
 */
export async function OperateRoleUserStatus(operate: string, record: RoleUserInfo): Promise<Resp> {
  switch (operate) {
    case 'deleteRoleBind':
      return request<Resp>('/api/b/permit/DeleteRoleBind?id=' + record.id, {
        method: 'POST',
      });
      break;
  }
  return new Promise<Resp>((resolve, reject) => {
    resolve({ ret: 0, is_failed: false, is_ok: true, msg: '' });
  });
}

/**
 * 添加用户角色绑定信息
 */
export async function addRoleBind(vals: { admin_info: AdminInfo; role_info: RoleInfo }) {
  return request<Resp>('/api/b/permit/AddRoleBind', {
    method: 'POST',
    data: {
      role_id: vals.role_info.id,
      u_id: vals.admin_info.u_id,
      u_name: vals.admin_info.admin_name,
    },
  });
}
