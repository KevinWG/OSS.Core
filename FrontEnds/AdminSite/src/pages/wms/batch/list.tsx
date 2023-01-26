import AccessButton from '@/components/button/access_button';
import TableFetchButtons from '@/components/button/table_Fetch_buttons';
import SearchProTable from '@/components/search/search_protable';
import { CommonStatus } from '@/services/common/enums';
import FuncCodes from '@/services/common/func_codes';
import { LockOutlined, UnlockOutlined } from '@ant-design/icons';
import { ActionType, PageContainer, ProColumns } from '@ant-design/pro-components';
import moment from 'moment';
import { useRef, useState } from 'react';
import AddBatch from './components/add_batch';
import { searchBatchList, setUseable } from './service';

export default function () {
  const statusButtons = [
    {
      condition: (r: WmsApi.BatchMo) => r.status == 0,
      buttons: [
        {
          title: '作废',
          fetch: (item: WmsApi.BatchMo) => setUseable(item['_pass_token_id'], 0),
          fetch_desp: '作废当前批次号',
          func_code: FuncCodes.wms_batch_modify,
          icon: <LockOutlined />,
        },
      ],
    },
    {
      condition: (r: WmsApi.BatchMo) => r.status == -100,
      buttons: [
        {
          title: '启用',
          fetch: (item: WmsApi.BatchMo) => setUseable(item['_pass_token_id'], 1),
          fetch_desp: '启用当前申请',
          func_code: FuncCodes.wms_batch_modify,
          icon: <UnlockOutlined />,
        },
      ],
    },
  ];

  const tableColumns: ProColumns<WmsApi.BatchMo>[] = [
    {
      title: '批次编码',
      dataIndex: 'code',
    },
    {
      title: '物料编码',
      dataIndex: 'material_code',
    },

    {
      title: '有效时间',
      dataIndex: 'expire_date',
      valueType: 'dateRange',

      render: (_, e) => moment(e.expire_date * 1000).format('YYYY-MM-DD'),
      search: {
        transform: (value) => {
          const dateRange = {
            expire_start_at: moment(value[0]).unix(),
            expire_end_at: moment(value[1]).unix(),
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
      title: '创建时间',
      dataIndex: 'add_time',
      hideInSearch: true,
      render: (_, e) => moment(e.add_time * 1000).format('YYYY-MM-DD'),
    },
    {
      title: '操作',
      dataIndex: 'id',
      hideInSearch: true,
      render: (_: any, r: WmsApi.BatchMo) => (
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

  const tableRef = useRef<ActionType>();
  const defaultItem = { id: '-1' } as WmsApi.BatchMo;
  const [editItem, setEditItem] = useState(defaultItem);

  return (
    <PageContainer>
      <SearchProTable<WmsApi.BatchMo>
        rowKey="id"
        size="small"
        columns={tableColumns}
        actionRef={tableRef}
        request={searchBatchList}
        toolbar={{
          actions: [
            <AccessButton
              type="primary"
              func_code={FuncCodes.wms_batch_modify}
              onClick={() => {
                setEditItem({ id: '0' } as any);
              }}
            >
              添加批次号
            </AccessButton>,
          ],
        }}
      />

      <AddBatch
        record={editItem}
        callback={(res) => {
          if (res.success) {
            tableRef.current?.reload();
            setEditItem(defaultItem);
          }
        }}
        drawerProps={{
          onClose: () => setEditItem(defaultItem),
        }}
      ></AddBatch>
    </PageContainer>
  );
}
