import { request } from 'umi';
import { Resp, PageListResp } from '@/utils/resp_d';
import { UserInfo } from './data_d';

export async function searchUsers(params: any) {
  return request<PageListResp<UserInfo>>('/api/user/SearchUsers', {
    method: 'POST',
    data: params,
  });
}

export async function lockUser(user: UserInfo, isLock: boolean) {
  const id = user.id;
  const apiUrl = isLock ? '/api/user/lock?id=' + id : '/api/user/unlock?id=' + id;
  return request<Resp>(apiUrl, {
    method: 'POST',
    data: {},
  });
}

export async function addUser(vals: any) {
  const apiUrl = '/api/user/adduser';
  return request<Resp>(apiUrl, {
    method: 'POST',
    data: vals,
  });
}

export async function checkRegName(vals: any) {
  const apiUrl = '/api/portal/CheckIfCanReg';
  return request<Resp>(apiUrl, {
    method: 'POST',
    data: vals,
  });
}
