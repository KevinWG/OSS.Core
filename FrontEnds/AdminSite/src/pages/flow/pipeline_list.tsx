import AccessButton from '@/components/button/access_button';
import TableFetchButtons from '@/components/button/table_Fetch_buttons';
import SearchProTable from '@/components/search/search_protable';
import { CommonStatus } from '@/services/common/enums';
import FuncCodes from '@/services/common/func_codes';
import { EditOutlined, LockOutlined } from '@ant-design/icons';
import { ActionType, PageContainer, ProColumns } from '@ant-design/pro-components';

import moment from 'moment';
import { useRef, useState } from 'react';
import Editor from './components/editor';

import { search, setUabale, StockDirection } from './service';

export default function PipelineList() {
  const statusButtons = [
    {
      condition: (r: WmsApi.StockApplyMo) => r.status == 0,
      buttons: [
        {
          title: '作废',
          fetch: (item: WmsApi.StockApplyMo) => setUabale(item['_pass_token_id'], 0),
          fetch_desp: '作废当前项',
          func_code: '',
          icon: <LockOutlined />,
        },
        {
          title: '编辑',
          icon: <EditOutlined />,
          func_code: FuncCodes.wms_stock_apply_manage,
          btn_click: (r: WmsApi.StockApplyMo) => {
            setEditItem(r);
          },
        },
      ],
    },
    {
      condition: (r: WmsApi.StockApplyMo) => r.status == -100,
      buttons: [
        {
          title: '启用',
          fetch: (item: WmsApi.StockApplyMo) => setUabale(item['_pass_token_id'], 1),
          fetch_desp: '启用当前项',
          func_code: '',
          icon: <LockOutlined />,
        },
      ],
    },
  ];

  const tableColumns: ProColumns<WmsApi.StockApplyMo>[] = [
    {
      title: '标题',
      dataIndex: 'title',
    },
    {
      title: '申请类型',
      dataIndex: 'direction',
      valueType: 'select',
      valueEnum: StockDirection,
    },
    {
      title: '计划时间',
      dataIndex: 'plan_time',
      valueType: 'dateRange',
      render: (_, e) => moment(e.plan_time * 1000).format('YYYY-MM-DD'),
      search: {
        transform: (value) => {
          const dateRange = {
            plan_start_at: moment(value[0]).unix(),
            plan_end_at: moment(value[1]).unix(),
          };
          return dateRange;
        },
      },
    },
    {
      title: '状态',
      dataIndex: 'status',
      valueType: 'select',
      valueEnum: CommonStatus,
    },
    {
      title: '操作',
      dataIndex: 'id',
      hideInSearch: true,
      render: (_: any, r: WmsApi.StockApplyMo) => (
        <TableFetchButtons
          record={r}
          callback={(res, item, aName) => {
            if (res.success) tableRef.current?.reload();
          }}
          fetchKey={(item) => item.id}
          condition_buttons={statusButtons}
        ></TableFetchButtons>
      ),
    },
  ];

  const defaultItem = { id: '-1' } as WmsApi.StockApplyMo;
  const [editItem, setEditItem] = useState(defaultItem);

  const tableRef = useRef<ActionType>();

  return (
    <PageContainer>
      <SearchProTable<WmsApi.StockApplyMo>
        rowKey="id"
        columns={tableColumns}
        actionRef={tableRef}
        request={search}
        toolbar={{
          actions: [
            <AccessButton
              type="primary"
              func_code={FuncCodes.wms_material_manage}
              onClick={() => {
                setEditItem({ id: '0' } as any);
              }}
            >
              添加申请
            </AccessButton>,
          ],
        }}
      />
      <Editor
        record={editItem}
        callback={(res) => {
          if (res.success) {
            setEditItem(defaultItem);
            tableRef.current?.reload();
          }
        }}
        drawerProps={{
          onClose: () => {
            setEditItem(defaultItem);
          },
        }}
      ></Editor>
    </PageContainer>
  );
}
