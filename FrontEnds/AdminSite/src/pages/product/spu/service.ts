import { request } from '@umijs/max';


export async function spu_search(params: any) {
    return request<IPageListResp<PortalApi.AdminInfo>>('/product/spu/msearch', {
      method: 'POST',
      data: params,
    });
}








