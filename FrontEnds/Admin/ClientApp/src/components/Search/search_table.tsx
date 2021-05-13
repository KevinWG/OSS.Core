import React, { useRef, useEffect } from 'react';
import { Table, Tabs, Card } from 'antd';
import { SearchReq, PageListResp, SearchFilterType, SearchOrderType } from '@/utils/resp_d';
import { useRequest } from 'umi';
import { TableProps, TablePaginationConfig } from 'antd/lib/table';
const { TabPane } = Tabs;

/**
 *  获取状态描述
 * @param state 当前状态
 * @param status  状态列表
 */
export function getTextFromTableStatus(
  state: number,
  status?: { label: string; value: string | number }[],
) {
  if (state==undefined) {
    return "无";
  }
  if (!status) {
    status = CommonStatus;
  }
  for (let index = 0; index < status.length; index++) {
    const element = status[index];
    if (state == element.value || state.toString() == element.value.toString()) {
      return element.label;
    }
  }
  return '无';
}

export const CommonStatus = [
  { label: '正常', value: 0 },
  { label: '已作废', value: -100 },
  // { label: '已结束', value: 10000 },
  { label: '全部', value: -999 },
];

export interface SearchTableAction {
  /**
   * 根据指定查询条件，重新加载表单
   * @param fromFirstPage 是否从新加载到首页，默认回到首页
   */
  reload: (filters: SearchFilterType, orders?: SearchOrderType, fromFirstPage?: boolean) => void;

  /**
   * 保持现有查询条件不变，刷新表单
   * @param fromFirstPage 是否回到首页,默认在当前页面
   */
  refresh: (fromFirstPage?: boolean) => void;
}

interface SearchTabelProps<RT>
  extends Omit<TableProps<RT>, 'dataSource' | 'loading' | 'pagination'> {
  search_fetch: (sReq: SearchReq) => Promise<PageListResp<RT>>;

  default_filters?: { [key: string]: string|number };
  default_orders?: { [pname: string]: 'ASC' | 'DESC' };

  search_table_ref?:
    | React.MutableRefObject<SearchTableAction | undefined>
    | ((tRef: SearchTableAction) => void);

  pagination?: Omit<TablePaginationConfig, 'total' | 'onChange' | 'onShowSizeChange'>;
}

function changeFilterValToString(val:{}){
  const nVal:{[key:string]:string}={};
  for (let k in val) {
    nVal[k] = val[k]?.toString();
    }
    return  nVal;   
  }
export default function SearchTable<T extends object>(props: SearchTabelProps<T>): JSX.Element {
  const { pagination, default_filters, default_orders, search_table_ref, ...restProps } = props;

  const tempSearchReq = useRef({
    current: pagination?.current || 1,
    size: pagination?.pageSize || 10,
    orders: default_orders,
    filter: default_filters || {},
  });

  const pageListReq = useRequest(() => 
  {
    const req=tempSearchReq.current;
    return   props.search_fetch({...req,filter:changeFilterValToString(req.filter)});
  });

  function pageChange(reqPage: number, reqSize?: number) {
    tempSearchReq.current.current = reqPage;
    pageListReq.run();
  }

  function sizeChange(_: number, s: number) {
    tempSearchReq.current.current = 1;
    tempSearchReq.current.size = s;
    pageListReq.run();
  }

  function refresh(fromFirstPage: boolean = false) {
    if (fromFirstPage) {
      tempSearchReq.current.current = 1;
    }
    pageListReq.run();
  }

  function reload(
    filter: SearchFilterType,
    orders?: SearchOrderType,
    fromFirstPage: boolean = true,
  ) {
    tempSearchReq.current.filter = filter;
    tempSearchReq.current.orders = orders;
    refresh(fromFirstPage);
  }

  useEffect(() => {
    const stRef = { reload: reload, refresh: refresh };
    if (search_table_ref && typeof search_table_ref === 'function') {
      search_table_ref(stRef);
    }
    if (search_table_ref && typeof search_table_ref !== 'function') {
      search_table_ref.current = stRef;
    }
  }, []);

  return (
    <Table<T>
      dataSource={pageListReq.data?.data}
      loading={pageListReq.loading}
      size="middle"
      pagination={{
        current: tempSearchReq.current.current,
        pageSize: tempSearchReq.current.size,
        total: pageListReq.data?.total,
        onChange: pageChange,
        onShowSizeChange: sizeChange,
        ...pagination,
      }}
      {...restProps}
    />
  );
}

interface SearchTabsTabelProps<RT> extends SearchTabelProps<RT> {
  tabs: {
    filter_name: string;
    default_key: string;
    options: { text: string; key: string }[];
  };
}

export function SearchTabsTable<T extends object>(props: SearchTabsTabelProps<T>): JSX.Element {
  const { tabs, default_filters, default_orders, search_table_ref, ...restProps } = props;

  const defaultTabFilter =
    tabs.filter_name && tabs.default_key
      ? { [tabs.filter_name]: tabs.default_key, ...default_filters }
      : default_filters;

  const tempDataRef = useRef({ filter: defaultTabFilter || {}, orders: default_orders });

  // 对外
  function reload(
    filters: SearchFilterType,
    orders?: SearchOrderType,
    fromFirstPage: boolean = true,
  ) {
    // 在外部filter的基础上，添加内部tab对应的filter
    if (!tabs.filter_name) {
      const keepKey = tempDataRef.current.filter[tabs.filter_name];
      tempDataRef.current.filter = { [tabs.filter_name]: keepKey, ...filters };
    } else {
      tempDataRef.current.filter = filters;
    }

    innerSearchTableRef.current?.reload(tempDataRef.current.filter, orders, fromFirstPage);
  }

  useEffect(() => {
    const stRef = {
      reload: reload,
      refresh: (fp?: boolean) => {
        innerSearchTableRef.current?.refresh(fp);
      },
    };
    if (search_table_ref && typeof search_table_ref === 'function') {
      search_table_ref(stRef);
    }
    if (search_table_ref && typeof search_table_ref !== 'function') {
      search_table_ref.current = stRef;
    }
  }, []);

  // 内部变化
  function tabChange(akey: string) {
    if (!tabs.filter_name) return;

    tempDataRef.current.filter[tabs.filter_name] = akey;
    innerSearchTableRef.current?.reload(tempDataRef.current.filter, tempDataRef.current.orders);
  }

  const innerSearchTableRef = useRef<SearchTableAction>();

  return (
    <Card size="small">
      <Tabs defaultActiveKey={props.tabs.default_key} onChange={tabChange} type="card">
        {props.tabs.options.map((o) => (
          <TabPane tab={<span>{o.text}</span>} key={o.key}></TabPane>
        ))}
      </Tabs>
      <SearchTable
        search_table_ref={innerSearchTableRef}
        {...restProps}
        default_filters={defaultTabFilter}
      ></SearchTable>
    </Card>
  );
}
