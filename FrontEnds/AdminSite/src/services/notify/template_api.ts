import { request } from '@umijs/max';

export function searchTemplate(req: SearchReq) {
  return request<IPageListResp<NotifyApi.Template>>('/notify/Template/Search', {
    method: 'POST',
    data: req,
  });
}

export function addTemplate(req: NotifyApi.AddTemplateReq) {
  return request<IResp>('/notify/Template/Add', {
    method: 'POST',
    data: req,
  });
}

export function updateTemplate(id: string, req: NotifyApi.AddTemplateReq) {
  return request<IResp>('/notify/Template/Update?id=' + id, {
    method: 'POST',
    data: req,
  });
}

export function setUseable(id: string, flag: number) {
  return request<IResp>('/notify/Template/SetUseable?id=' + id + '&flag=' + flag, {
    method: 'POST',
  });
}

export const NotifyType = [
  {
    value: 10,
    label: '邮箱',
  },
  {
    value: 20,
    label: '短信',
  },
];

export const NotifyChannel = [
  {
    value: 1,
    label: '系统通道',
  },
  {
    value: 2,
    label: '系统测试通道',
  },
  {
    value: 10,
    label: '阿里云通道',
  },
  {
    value: 20,
    label: '华为云',
  },
];
