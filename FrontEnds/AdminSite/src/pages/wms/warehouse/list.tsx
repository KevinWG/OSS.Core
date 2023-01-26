import AccessButton from '@/components/button/access_button';
import TableButton from '@/components/button/table_button';
import TableFetchButtons from '@/components/button/table_Fetch_buttons';
import { CommonStatus } from '@/services/common/enums';
import FuncCodes from '@/services/common/func_codes';
import {
  AppstoreOutlined,
  EditOutlined,
  LockOutlined,
  PlusOutlined,
  UnlockOutlined,
} from '@ant-design/icons';
import { PageContainer, ProColumns, ProTable } from '@ant-design/pro-components';
import { useRequest } from 'ahooks';
import { Switch } from 'antd';
import { useState } from 'react';

import EditWarehouse from './components/warehouse_edit';

import moment from 'moment';
import AreaList from './components/area_list';
import { getAllWarehouse, setUabale } from './service';

export default function list() {
  const wListReq = useRequest(getAllWarehouse);

  const statusButtons = [
    {
      condition: (r: WmsApi.Warehouse) => r.status == 0,
      buttons: [
        {
          title: '禁用',
          fetch: (item: WmsApi.Warehouse) => setUabale(item.id, 0),
          fetch_desp: '禁用当前仓库',
          func_code: FuncCodes.WMS_Warehouse_Manage,
          icon: <LockOutlined />,
        },
        {
          title: '编辑',
          icon: <EditOutlined />,
          func_code: FuncCodes.WMS_Warehouse_Manage,
          btn_click: (r: WmsApi.Warehouse) => {
            setWarehouse(r);
          },
        },
        {
          title: '管理库位',
          icon: <AppstoreOutlined />,
          func_code: FuncCodes.WMS_WareArea,
          btn_click: (r: WmsApi.Warehouse) => {
            setAreaWarehouse(r);
          },
        },
      ],
    },
    {
      condition: (r: WmsApi.Warehouse) => r.status == -100,
      buttons: [
        {
          title: '解禁',
          fetch: (item: WmsApi.Warehouse) => setUabale(item.id, 1),
          fetch_desp: '解禁当前仓库',
          func_code: FuncCodes.WMS_Warehouse_Manage,
          icon: <UnlockOutlined />,
        },
      ],
    },
  ];

  const tableColumns: ProColumns<WmsApi.Warehouse>[] = [
    {
      title: '名称',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: '备注',
      dataIndex: 'remark',
      key: 'remark',
    },
    {
      title: '状态',
      dataIndex: 'status',
      key: 'status',
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
      render: (_, r) => (
        <TableFetchButtons
          record={r}
          callback={(res, item, aName) => {
            if (res.success) wListReq.run(withAll);
          }}
          fetchKey={(item) => item.id}
          condition_buttons={statusButtons}
        >
          {r.parent_id.toString() == '0' && (
            <TableButton
              func_code={FuncCodes.WMS_Warehouse_Manage}
              onClick={() => {
                setParentWarehouse(r);
                setWarehouse({ id: '0' } as any);
              }}
              title="添加子仓"
              icon={<PlusOutlined />}
            ></TableButton>
          )}
        </TableFetchButtons>
      ),
    },
  ];

  const defaultWarehouse = { id: '-1' } as WmsApi.Warehouse;

  const [withAll, setWithAll] = useState(true);

  const [warehouse, setWarehouse] = useState(defaultWarehouse);
  const [parentWarehouse, setParentWarehouse] = useState(defaultWarehouse);

  const [areaWarehouse, setAreaWarehouse] = useState(defaultWarehouse);

  function close() {
    setWarehouse(defaultWarehouse);
    setParentWarehouse(defaultWarehouse);
  }

  return (
    <PageContainer>
      {wListReq.data && (
        <ProTable<WmsApi.Warehouse>
          size="small"
          search={false}
          pagination={false}
          toolbar={{
            actions: [
              <AccessButton
                size="small"
                func_code={FuncCodes.WMS_Warehouse_Manage}
                onClick={() => {
                  setWarehouse({ id: '0' } as any);
                }}
                icon={<PlusOutlined />}
              >
                添加一级仓库
              </AccessButton>,
              <Switch
                checkedChildren="含禁用"
                unCheckedChildren="不含禁用"
                defaultChecked={withAll}
                onChange={(v) => {
                  setWithAll(v);
                  wListReq.run(v);
                }}
              />,
            ],
          }}
          defaultExpandAllRows={true}
          indentSize={10}
          loading={wListReq.loading}
          dataSource={wListReq.data.data}
          columns={tableColumns}
        ></ProTable>
      )}
      <EditWarehouse
        parent={parentWarehouse}
        record={warehouse}
        callback={(res) => {
          if (res.success) {
            wListReq.run(withAll);
            close();
          }
        }}
        visible={warehouse.id != '-1'}
        onCancel={close}
      ></EditWarehouse>

      <AreaList
        width={800}
        warehouse={areaWarehouse}
        visible={areaWarehouse.id != '-1'}
        onClose={() => {
          setAreaWarehouse(defaultWarehouse);
        }}
      ></AreaList>
    </PageContainer>
  );
}
