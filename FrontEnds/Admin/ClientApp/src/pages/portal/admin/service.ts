import { request } from 'umi';
import { RespData, Resp, PageListResp } from '@/utils/resp_d';
import { AdminInfo, AdminCreateReq } from './data_d';
// 管理员列表
export async function searchAdmins(params: any) {
  return request<PageListResp<AdminInfo>>('/api/admin/SearchAdmins', {
    method: 'POST',
    data: params,
  });
}

export async function lockAdmin(admin: AdminInfo, isLock: boolean) {
  const id = admin.u_id;
  const apiUrl = '/api/admin/' + (isLock ? 'lock' : 'unlock') + '?uid=' + id;
  return request<Resp>(apiUrl, {
    method: 'POST',
    data: {},
  });
}

// 创建管理员

export async function AdminCreate(req: AdminCreateReq) {
  const apiUrl = '/api/admin/create';
  return request<Resp>(apiUrl, {
    method: 'POST',
    data: req,
  });
}

// 登录用户相关操作
export function getAvatarUploadPara() {
  return request<RespData<GetUploadResp>>('/api/upload/AvatarUploadPara');
}

export function updateNewAvatar(avatarImg: string) {
  return request<Resp>('/api/admin/ChangeOwnerAvatar?avatar=' + avatarImg, {
    method: 'post',
  });
}

export async function setAdminType(admin: AdminInfo, adminType: number) {
  const id = admin.u_id;
  const apiUrl = '/api/admin/setAdminType?uid=' + id + '&admin_type=' + adminType;
  return request<Resp>(apiUrl, {
    method: 'POST',
    data: {},
  });
}
