import { request } from '@umijs/max';

export async function searchMaterials(searchReq: SearchReq) {
  return request<IPageListResp<WmsApi.Material>>('/wms/Material/MSearch', {
    method: 'POST',
    data: searchReq,
  });
}

export async function setUabale(id: string, flag: number) {
  return request<IResp>('/wms/Material/SetUseable?id=' + id + '&flag=' + flag, {
    method: 'POST',
  });
}

export async function addMaterial(req: WmsApi.AddCategoryReq) {
  return request<IResp>('/wms/Material/Add', {
    method: 'POST',
    data: req,
  });
}

export async function updateMaterial(id: string, req: WmsApi.AddCategoryReq) {
  return request<IResp>('/wms/Material/update?id=' + id, {
    method: 'POST',
    data: req,
  });
}

export const MaterialType = {
  '0': {
    text: '原材料',
  },
  '100': {
    text: '半成品',
  },
  '200': {
    text: '成品',
  },
};

export async function getMSkuList(mId: string) {
  return request<IPageListResp<WmsApi.MSku>>('/wms/MSku/MList?m_id=' + mId);
}

export async function setMSkuUabale(pass_token: string, flag: number) {
  return request<IResp>('/wms/MSku/SetUseable?pass_token=' + pass_token + '&flag=' + flag, {
    method: 'POST',
  });
}

export async function addMSku(req: WmsApi.AddMSkuReq) {
  return request<IResp>('/wms/MSku/add', {
    method: 'POST',
    data: req,
  });
}

export async function editMSku(pass_token: string, req: WmsApi.AddMSkuReq) {
  return request<IResp>('/wms/MSku/edit?pass_token=' + pass_token, {
    method: 'POST',
    data: req,
  });
}
