import { request } from '@umijs/max';

export async function searchOrg(searchReq: SearchReq) {
  return request<IPageListResp<PortalApi.Orgnization>>('/portal/Organization/MSearch', {
    method: 'POST',
    data: searchReq,
  });
}

export async function comSearchOrg(searchReq: SearchReq) {
  return request<IPageListResp<PortalApi.Orgnization>>('/portal/Organization/ComSearch', {
    method: 'POST',
    data: searchReq,
  });
}

export async function setOrgUabale(token: string, flag: number) {
  return request<IResp>('/portal/Organization/SetUseable?pass_token=' + token + '&flag=' + flag, {
    method: 'POST',
  });
}

export async function addOrg(req: PortalApi.AddOrgnizationReq) {
  return request<IResp>('/portal/Organization/Add', {
    method: 'POST',
    data: req,
  });
}

export const OrgType = {
  '0': {
    text: '客户',
  },
  '100': {
    text: '供应商',
  },
  '200': {
    text: '加工制造商',
  },
  '1000': {
    text: '其他',
  },
};
