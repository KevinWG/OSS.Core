import { request } from '@umijs/max';

export function saveAuthSetting(req: PortalApi.AuthSetting) {
  return request<IResp>('/portal/Setting/SaveAuthSetting', {
    method: 'POST',
    data: req,
  });
}

export function getAuthSetting() {
  return request<IResp>('/portal/Setting/GetAuthSetting');
}
