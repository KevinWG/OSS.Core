import { request } from '@umijs/max';

export async function searchUsers(params: SearchReq) {
  return request<IPageListResp<PortalApi.UserInfo>>('/portal/user/searchusers', {
    method: 'POST',
    data: params,
  });
}

export async function lockUser(id: string, isLock: boolean) {
  const apiUrl = isLock ? '/portal/user/lock?id=' + id : '/portal/user/unlock?id=' + id;
  return request<IResp>(apiUrl, {
    method: 'POST',
    data: {},
  });
}

export async function getUser(id: string) {
  return request<IRespData<PortalApi.UserInfo>>('/portal/user/get?id=' + id, {
    method: 'GET',
  });
}
