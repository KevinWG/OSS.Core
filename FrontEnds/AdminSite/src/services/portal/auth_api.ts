import { request } from '@umijs/max';
import { PortalNameType } from './enums';

/** 获取当前的用户信息 GET /api/currentUser */
export async function getUserIdentityAndAccess() {
  var identityRes = await request<IRespData<PortalApi.UserIdentity>>('/portal/Auth/GetIdentity', {
    method: 'GET',
  });
  if (!identityRes.success) return identityRes;

  var funcRes = await request<IRespData<PortalApi.AccessFunItem[]>>(
    '/portal/Grant/GetCurrentUserPermits',
    {
      method: 'GET',
    },
  );
  if (funcRes.success) {
    identityRes.data.access_list = funcRes.data;
  }
  return identityRes;
}

export function postPwdAdminLogin(req: PortalApi.PasswordAuthReq) {
  return request<PortalApi.AuthResp>('/portal/Auth/PwdAdminLogin', {
    method: 'POST',
    data: req,
  });
}

export function postCodeAdminLogin(req: PortalApi.CodeAuthReq) {
  return request<PortalApi.AuthResp>('/portal/Auth/CodeAdminLogin', {
    method: 'POST',
    data: req,
  });
}

export function getLogout() {
  return request<IResp>('/portal/Auth/Logout', {
    method: 'GET',
  });
}

export function getLoginNameType(loginName: string) {
  if (/^1\d{10}$/.test(loginName)) {
    return PortalNameType.Mobile; // 手机号登录
  } else if (/^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/.test(loginName)) {
    return PortalNameType.Email; // 邮箱登录
  } else {
    return undefined; // 未知
  }
}

export function postSendCode(req: PortalApi.IPortalNameReq) {
  return request<IResp>('/portal/Auth/SendCode', {
    method: 'POST',
    data: req,
  });
}

export function checkRegName(vals: any) {
  const apiUrl = '/portal/Auth/CheckIfCanReg';
  return request<IResp>(apiUrl, {
    method: 'POST',
    data: vals,
  });
}

export async function addUser(vals: any) {
  const apiUrl = '/portal/AuthBinding/AddUser';
  return request<IResp>(apiUrl, {
    method: 'POST',
    data: vals,
  });
}
