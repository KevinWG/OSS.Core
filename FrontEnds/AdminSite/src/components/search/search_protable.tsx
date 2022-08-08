import { ParamsType, ProTable, ProTableProps, RequestData } from '@ant-design/pro-components';
import { SortOrder } from 'antd/lib/table/interface';

export interface SearchProTableProps<T, U, ValueType>
  extends Omit<ProTableProps<T, U, ValueType>, 'request'> {
  request?: (req: SearchReq) => Promise<Partial<RequestData<T>>>;
}
function SearchProTable<
  DataType extends Record<string, any>,
  Params extends ParamsType = ParamsType,
  ValueType = 'text',
>({ request, ...restProps }: SearchProTableProps<DataType, Params, ValueType>) {
  const proRequest = request
    ? async (
        params: Params & {
          pageSize?: number;
          current?: number;
          keyword?: string;
        },
        sort: Record<string, SortOrder>,
        filter: Record<string, React.ReactText[] | null>,
      ) => {
        const { pageSize, current, keyword, ...restParas } = params;

        const searchReq: SearchReq = { current: current || 1, size: pageSize || 20 };

        const searchReqFilter = keyword ? { keyword: keyword, ...restParas } : restParas;
        searchReq.filter = changeFilterValToString(searchReqFilter);

        searchReq.orders = changeSort(sort);

        const result = await request(searchReq);

        if (result.success && result.data && result.pass_tokens) {
          for (let keyName in result.pass_tokens) {
            result.data.forEach((item) => {
              var relateTokenKeyValue = item[keyName];
              (item as any)['_pass_token_' + keyName] =
                result.pass_tokens[keyName][relateTokenKeyValue];
            });
          }
        }

        return result;
      }
    : undefined;

  return (
    <ProTable<DataType, Params, ValueType>
      request={proRequest}
      search={{ defaultCollapsed: false }}
      {...restProps}
    ></ProTable>
  );
}

export default SearchProTable;

function changeFilterValToString(val: {}) {
  const nVal: { [key: string]: string } = {};
  for (let k in val) {
    nVal[k] = val[k]?.toString();
  }
  return nVal;
}

function changeSort(sort: Record<string, SortOrder>) {
  const nVal: { [key: string]: 'ASC' | 'DESC' } = {};
  for (let k in sort) {
    nVal[k] = sort[k]?.toString() == 'ascend' ? 'ASC' : 'DESC';
  }
  return nVal;
}

/**
 *  获取状态描述
 * @param state 当前状态
 * @param status  状态列表
 */
export function getTextFromLabelValues(
  state: number,
  status?: { label: string; value: string | number }[],
) {
  if (state == undefined || !status) {
    return '无';
  }
  for (let index = 0; index < status.length; index++) {
    const element = status[index];
    if (state == element.value || state.toString() == element.value.toString()) {
      return element.label;
    }
  }
  return '无';
}
