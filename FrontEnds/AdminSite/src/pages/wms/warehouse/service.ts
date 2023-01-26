import { request } from '@umijs/max';

export async function getAllWarehouse(withAll: boolean) {
  return request<IListResp<WmsApi.Warehouse>>(
    '/wms/warehouse/' + (withAll ? 'all' : 'AllUseable'),
  ).then((listRes: any) => {
    if (listRes.success) {
      var list = [];
      for (let index = 0; index < listRes.data.length; index++) {
        const element = listRes.data[index];
        if (element.parent_id.toString() == '0') {
          element.children = listRes.data.filter((w) => w.parent_id == element.id);
          list.push(element);
        }
      }
      listRes.data = list;
    }
    return listRes;
  });
}

export async function setUabale(id: string, flag: number) {
  return request<IListResp<WmsApi.WareArea>>(
    '/wms/warehouse/SetUseable?id=' + id + '&flag=' + flag,
    {
      method: 'POST',
    },
  );
}

export async function getAreaList(w_id: string) {
  return request<IListResp<WmsApi.WareArea>>('/wms/warehouse/AreaList?w_id=' + w_id);
}

export async function add(req: WmsApi.AddWarehouseReq) {
  return request<IListResp<WmsApi.WareArea>>('/wms/warehouse/add', {
    method: 'POST',
    data: req,
  });
}

export async function update(id: string, req: WmsApi.UpdateWarehouseReq) {
  return request<IListResp<WmsApi.WareArea>>('/wms/warehouse/update?id=' + id, {
    method: 'POST',
    data: req,
  });
}

// ======= Area

export const TradeFlag = {
  '-100': {
    text: '禁止',
    status: 'Error',
  },
  '0': {
    text: '允许',
    status: 'Success',
  },
};

export async function addArea(req: WmsApi.AddWareAreaReq) {
  return request<IListResp<WmsApi.WareArea>>('/wms/warehouse/AddArea', {
    method: 'POST',
    data: req,
  });
}

export async function updateArea(id: string, req: WmsApi.UpdateWareAreaReq) {
  return request<IListResp<WmsApi.WareArea>>('/wms/warehouse/UpdateArea?id=' + id, {
    method: 'POST',
    data: req,
  });
}

export async function setAreaUabale(id: string, flag: number) {
  return request<IListResp<WmsApi.WareArea>>(
    '/wms/warehouse/SetAreaUseable?id=' + id + '&flag=' + flag,
    {
      method: 'POST',
    },
  );
}
export async function checkAreaCode(wId: string, code: string) {
  console.info(code);
  const res = await request<IResp>('/wms/warehouse/CheckAreaCode', {
    method: 'POST',
    data: { w_id: wId, code: code },
  });
  if (res.success) {
    return Promise.resolve();
  } else {
    return Promise.reject(res.msg);
  }
}

export async function setAreaTradeFlag(id: string, flag: number) {
  return request<IListResp<WmsApi.WareArea>>(
    '/wms/warehouse/SetAreaTradeFlag?id=' + id + '&flag=' + flag,
    {
      method: 'POST',
    },
  );
}
