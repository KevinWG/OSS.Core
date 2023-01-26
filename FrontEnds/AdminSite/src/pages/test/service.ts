import { request } from '@umijs/max';

export async function search(searchReq: SearchReq) {
  return request<IPageListResp<WmsApi.StockApplyMo>>('/wms/StockApply/MSearch', {
    method: 'POST',
    data: searchReq,
  });
}

export async function setUabale(token: string, flag: number) {
  return request<IResp>('/wms/StockApply/SetUseable?pass_token=' + token + '&flag=' + flag, {
    method: 'POST',
  });
}

export async function add(req: WmsApi.AddStockApplyReq) {
  return request<IResp>('/wms/StockApply/Add', {
    method: 'POST',
    data: req,
  });
}

export async function edit(pass_token: string, req: WmsApi.AddStockApplyReq) {
  return request<IResp>('/wms/StockApply/Edit?pass_token=' + pass_token, {
    method: 'POST',
    data: req,
  });
}

export const StockDirection = {
  '10': {
    text: '入库申请',
  },
  '20': {
    text: '出库申请',
  },
};
