import { request } from '@umijs/max';

export async function category_search() {
  return request<IPageListResp<ProductApi.CategoryMo>>('/product/SpuCategory/AllCategories');
}

export function abandonCategory(c: ProductApi.CategoryMo) {
  return request<IPageListResp<ProductApi.CategoryMo>>(
    '/product/SpuCategory/SetUseable?id=' + c.id + '&flag=0',
    {
      method: 'POST',
    },
  );
}

export function createCategory(c: ProductApi.AddCategoryReq) {
  return request<IPageListResp<ProductApi.CategoryMo>>('/product/SpuCategory/add', {
    method: 'POST',
    data: c,
  });
}

export function updateCategorName(id: string, c: ProductApi.UpdateCategoryReq) {
  return request<IResp>('/product/SpuCategory/updatename?id=' + id, {
    method: 'POST',
    data: c,
  });
}
export function updateCategorOrder(id: string, c: ProductApi.UpdateCategoryReq) {
  return request<IResp>('/product/SpuCategory/updateorder?id=' + id + '&order=' + c.order, {
    method: 'POST',
    // data: ,
  });
}
