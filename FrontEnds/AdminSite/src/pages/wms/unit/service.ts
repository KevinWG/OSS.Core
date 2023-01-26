import { request } from '@umijs/max';

export async function getAllUnits() {
  return request<IListResp<WmsApi.Material>>('/wms/Unit/All');
}
export async function addUnit(req: { name: string }) {
  return request<IResp>('/wms/Unit/Add', {
    method: 'POST',
    data: req,
  });
}
