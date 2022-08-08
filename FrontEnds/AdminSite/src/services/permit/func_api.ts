import { request } from '@umijs/max';

export function getAllFuncItems() {
  return request<IListResp<PermitApi.FuncItem>>('/portal/Func/GetAllFuncItems');
}
export async function getAllFuncTreeItems() {
  var funcItemRes = await request<IListResp<PermitApi.FuncItem>>('/portal/Func/GetAllFuncItems');
  if (funcItemRes.success) {
    funcItemRes.data = ToIndentedFunItem(funcItemRes.data, undefined);
  }
  return funcItemRes;
}

function ToIndentedFunItem(arr: PermitApi.FuncItem[], parentCode?: string) {
  const list: PermitApi.FuncTreeItem[] = [];

  for (const item of arr) {
    if (item.parent_code == parentCode || (!parentCode && !item.parent_code)) {
      const treeItem: PermitApi.FuncTreeItem = { ...item };
      treeItem.children = ToIndentedFunItem(arr, item.code);
      list.push(treeItem);
    }
  }
  return list;
}

export function addFuncItem(addReq: PermitApi.AddFuncItemReq) {
  return request<IResp>('/portal/Func/AddFuncItem', {
    method: 'POST',
    data: addReq,
  });
}

export function changeFuncItem(code: string, req: PermitApi.ChangeFuncItemReq) {
  return request<IResp>('/portal/Func/ChangeFuncItem?code=' + code, {
    method: 'POST',
    data: req,
  });
}

export function setUseable(code: string, flag: number) {
  return request<IResp>('/portal/Func/SetUseable?code=' + code + '&flag=' + flag, {
    method: 'POST',
  });
}
