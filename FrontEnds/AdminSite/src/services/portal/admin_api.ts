import { request } from '@umijs/max';

// 管理员列表
export async function searchAdmins(params: any) {
  return request<IPageListResp<PortalApi.AdminInfo>>('/portal/admin/searchadmins', {
    method: 'POST',
    data: params,
  });
}

export async function lockAdmin(admin: PortalApi.AdminInfo, isLock: boolean) {
  const id = admin.id;
  const apiUrl = '/portal/admin/' + (isLock ? 'lock' : 'unlock') + '?uid=' + id;
  return request<IResp>(apiUrl, {
    method: 'POST',
    data: {},
  });
}

// 创建管理员
export async function AdminCreate(req: PortalApi.CreateAdminReq) {
  const apiUrl = '/portal/admin/create';
  return request<IResp>(apiUrl, {
    method: 'POST',
    data: req,
  });
}

export function updateNewAvatar(avatarImg: string) {
  return request<IResp>('/portal/admin/ChangeMyAvatar?avatar=' + avatarImg, {
    method: 'post',
  });
}

export function changeMyName(name: string) {
  return request<IResp>('/portal/admin/ChangeMyName?name=' + name, {
    method: 'post',
  });
}

export async function setAdminType(admin: PortalApi.AdminInfo, adminType: number) {
  const id = admin.id;
  const apiUrl = '/portal/admin/SetAdminType?uid=' + id + '&admin_type=' + adminType;
  return request<IResp>(apiUrl, {
    method: 'POST',
    data: {},
  });
}
