import { request } from '@umijs/max';

export async function category_search() {
  return request<IListResp<WmsApi.CategoryMo>>('/wms/mcategory/AllCategories');
}

export function abandonCategory(c: WmsApi.CategoryMo) {
  return request<IPageListResp<WmsApi.CategoryMo>>(
    '/wms/mcategory/SetUseable?id=' + c.id + '&flag=0',
    {
      method: 'POST',
    },
  );
}

export function createCategory(c: WmsApi.AddCategoryReq) {
  return request<IPageListResp<WmsApi.CategoryMo>>('/wms/mcategory/add', {
    method: 'POST',
    data: c,
  });
}

export function updateCategorName(id: string, c: WmsApi.UpdateCategoryReq) {
  return request<IResp>('/wms/mcategory/updatename?id=' + id, {
    method: 'POST',
    data: c,
  });
}

export function updateCategorOrder(id: string, c: WmsApi.UpdateCategoryReq) {
  return request<IResp>('/wms/mcategory/updateorder?id=' + id + '&order=' + c.order, {
    method: 'POST',
    // data: ,
  });
}
