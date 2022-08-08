interface SearchReq {
  current: number;
  size: number;
  orders?: SearchOrderType;
  filter?: SearchFilterType;
}
interface SearchFilterType {
  [pname: string]: string;
}
interface SearchOrderType {
  [pname: string]: 'ASC' | 'DESC';
}

//== 通用结果实体
interface IResp {
  success: boolean;
  code: number;
  msg: string;
}

/**  结果响应实体 */
interface IRespData<T> extends IResp {
  data: T;
}

interface IListResp<T> extends IRespData<T[]> {
  pass_tokens: { [key: string]: { [key: string]: string } };
}

interface IPageListResp<T> extends IListResp<T> {
  total: number;
}

interface BaseMo {
  id: string;
  status: number;
  add_time: number;
}

interface IEditorProps<T> {
  record: T;
  call_back?: (resp: IResp, record: T) => void;
}
