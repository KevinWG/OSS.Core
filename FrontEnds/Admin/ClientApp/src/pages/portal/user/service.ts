import { request } from 'umi';
import { Resp, PageListResp } from '@/utils/resp_d';
import { UserInfo } from './data_d';

export async function searchUsers(params: any) {
  return request<PageListResp<UserInfo>>('/api/b/user/searchusers', {
    method: 'POST',
    data: params,
  });
}

export async function lockUser(user: UserInfo, isLock: boolean) {
  const id = user.id;
  const apiUrl = isLock ? '/api/b/user/lock?id=' + id : '/api/b/user/unlock?id=' + id;
  return request<Resp>(apiUrl, {
    method: 'POST',
    data: {},
  });
}

export async function addUser(vals: any) {
  const apiUrl = '/api/b/user/AddUser';
  return request<Resp>(apiUrl, {
    method: 'POST',
    data: vals,
  });
}

export async function checkRegName(vals: any) {
  const apiUrl = '/api/b/portal/CheckIfCanReg';
  return request<Resp>(apiUrl, {
    method: 'POST',
    data: vals,
  });
}
