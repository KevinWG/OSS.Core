import FuncCodes from '../../config/func_codes';

export { FuncCodes };

export interface BaseMo {
  id: string;
  status: number;
  add_time: number;
}

export interface Resp {
  is_ok?: boolean;
  is_failed?: boolean; // 和 is_ok 相反
  ret: number | undefined;
  msg: string | undefined;
}

export interface ListResp<T> extends Resp {
  data: T[];
}

export interface RespData<T> extends Resp {
  data: T;
}

export interface PageListResp<T> extends RespData<T[]> {
  total: number | undefined;
}

export interface SearchReq {
  current: number;
  size: number;
  orders?: { [pname: string]: 'ASC' | 'DESC' };
  filter: { [pname: string]: string };
}
export interface SearchFilterType {
  [pname: string]: string;
}
export interface SearchOrderType {
  [pname: string]: 'ASC' | 'DESC';
}
export interface ProTableReqParams {
  current?: number;
  pageSize?: number;
  _timestamp?: number | undefined;
}
