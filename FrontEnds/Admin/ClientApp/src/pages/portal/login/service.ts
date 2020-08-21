import { request } from 'umi';
import { RespData, Resp } from '@/utils/resp_d';

export interface AdminIdentity {
  id: string;
  name: string | undefined;
  avatar: string | undefined;
  access_list: AccessItem[];
}

export interface AccessItem {
  func_code: string;
  data_level: number;
}

export interface LoginStateType {
  status?: 'ok' | 'error';
  type?: string;
}

//#region  登录相关接口实体处理
export interface LoginParamsType {
  userName: string;
  password: string;
  mobile: string;
  captcha: string;
  type: string;
}

async function adminLogin(url: string, bodyData: any) {
  var loginRes = await request<RespData<AdminIdentity>>(url, {
    method: 'post',
    data: bodyData,
  });
  return loginRes;
}

function getAuthAdminFuncList() {
  return request<RespData<AccessItem[]>>('/api/permit/GetAuthUserFuncList');
}

export async function getAuthAdminWithAccess() {
  var adminRes = await request<RespData<AdminIdentity>>('/api/portal/GetAuthAdmin');
  if (adminRes.is_failed) {
    return adminRes;
  }
  var accessRes = await getAuthAdminFuncList();
  if (accessRes.is_ok) {
    adminRes.data.access_list = accessRes.data;
  }
  return adminRes;
}

export function adminUserCodeLogin(bodyData: any) {
  return adminLogin('/api/portal/AdminCodeLogin', bodyData);
}
export async function adminUserPasswordLogin(bodyData: any) {
  return await adminLogin('/api/portal/AdminPasswordLogin', bodyData);
}

export async function outLogin() {
  return request('/api/portal/quit');
}

export function sendCode(type: number, name: string) {
  return request<Resp>('/api/portal/SendCode', {
    method: 'post',
    data: { type, name },
  });
}

export function getLoginType(loginName: string) {
  if (/^1\d{10}$/.test(loginName)) {
    return 20; // 手机号登录
  } else if (/^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test(loginName)) {
    return 30; // 邮箱登录
  } else {
    return 0; // 未知
  }
}
