import { request } from '@umijs/max';

export async function searchBatchList(searchReq: SearchReq) {
  return request<IPageListResp<WmsApi.BatchMo>>('/wms/batch/MSearch', {
    method: 'POST',
    data: searchReq,
  });
}

export async function addBatch(addReq: WmsApi.AddBatchReq) {
  return request<IResp>('/wms/batch/Add', {
    method: 'POST',
    data: addReq,
  });
}

export async function setUseable(pass_token: string, flag: number) {
  return request<IResp>(`/wms/batch/SetUseable?pass_token=` + pass_token + '&flag=' + flag, {
    method: 'POST',
  });
}
