import AccessButton from '@/components/button/access_button';
import TableFetchButtons from '@/components/button/table_Fetch_buttons';
import { CommonStatus } from '@/services/common/enums';
import FuncCodes from '@/services/common/func_codes';
import {
  CheckCircleOutlined,
  EditOutlined,
  LockOutlined,
  StopOutlined,
  UnlockOutlined,
} from '@ant-design/icons';
import { ProColumns, ProTable } from '@ant-design/pro-components';
import { useRequest } from 'ahooks';
import { Drawer, DrawerProps, Space } from 'antd';
import { useEffect, useState } from 'react';
import { getAreaList, setAreaTradeFlag, setAreaUabale, TradeFlag } from '../service';
import AreaEditor from './area_editor';

interface AreaListProps extends DrawerProps {
  warehouse: WmsApi.Warehouse;
}

export default function AreaList({ warehouse, ...respProps }: AreaListProps) {
  const areaListReq = useRequest(getAreaList, { manual: true });
  const statusButtons = [
    {
      condition: (r: WmsApi.WareArea) => r.status == 0,
      buttons: [
        {
          title: '禁用',
          fetch: (item: WmsApi.WareArea) => setAreaUabale(item.id, 0),
          fetch_desp: '禁用当前区位',
          func_code: FuncCodes.WMS_WareArea_Manage,
          icon: <LockOutlined />,
        },
        {
          title: '编辑',
          icon: <EditOutlined />,
          func_code: FuncCodes.WMS_WareArea_Manage,
          btn_click: (r: WmsApi.WareArea) => {
            setEditArea(r);
          },
        },
      ],
    },
    {
      condition: (r: WmsApi.WareArea) => r.status == -100,
      buttons: [
        {
          title: '解禁',
          func_code: FuncCodes.WMS_WareArea_Manage,
          fetch: (item: WmsApi.WareArea) => setAreaUabale(item.id, 1),
          fetch_desp: '解禁当前仓库',
          icon: <UnlockOutlined />,
        },
      ],
    },
  ];

  const tradeFlagButtons = [
    {
      condition: (r: WmsApi.WareArea) => r.trade_flag == 0,
      buttons: [
        {
          title: '禁止交易',
          btn_text: '禁止',
          func_code: FuncCodes.WMS_WareArea_Manage,
          fetch: (item: WmsApi.WareArea) => setAreaTradeFlag(item.id, -100),
          fetch_desp: '设置禁止交易',
          // func_code: FuncCodes.portal_user_lock,
          icon: <StopOutlined />,
        },
      ],
    },
    {
      condition: (r: WmsApi.WareArea) => r.trade_flag == -100,
      buttons: [
        {
          title: '允许交易',
          btn_text: '允许',
          func_code: FuncCodes.WMS_WareArea_Manage,
          fetch: (item: WmsApi.WareArea) => setAreaTradeFlag(item.id, 0),
          fetch_desp: '设置允许交易',
          icon: <CheckCircleOutlined />,
        },
      ],
    },
  ];

  const areaTableColumns: ProColumns<WmsApi.WareArea>[] = [
    {
      title: '编码',
      dataIndex: 'code',
      key: 'code',
    },
    {
      title: '交易状态',
      dataIndex: 'trade_flag',
      valueType: 'select',
      valueEnum: TradeFlag,
      render: (ele, r) => (
        <Space>
          {ele}
          <TableFetchButtons
            record={r}
            callback={(res, item, aName) => {
              if (res.success) areaListReq.run(warehouse.id);
            }}
            fetchKey={(item) => item.id}
            condition_buttons={tradeFlagButtons}
          ></TableFetchButtons>
        </Space>
      ),
    },
    {
      title: '使用状态',
      dataIndex: 'status',
      key: 'status',
      valueType: 'select',
      valueEnum: CommonStatus,
    },
    {
      title: '备注',
      dataIndex: 'remark',
      key: 'remark',
    },
    {
      title: '操作',
      dataIndex: 'id',
      hideInSearch: true,
      render: (_, r) => (
        <TableFetchButtons
          record={r}
          callback={(res, item, aName) => {
            if (res.success) areaListReq.run(warehouse.id);
          }}
          fetchKey={(item) => item.id}
          condition_buttons={statusButtons}
        ></TableFetchButtons>
      ),
    },
  ];

  useEffect(() => {
    if (warehouse && warehouse.id != '-1') {
      areaListReq.run(warehouse.id);
    }
  }, [warehouse]);

  const defaultArea = { id: '-1' } as WmsApi.WareArea;
  const [editArea, setEditArea] = useState(defaultArea);

  return (
    <Drawer title={warehouse.parent_name + ' / ' + warehouse.name + '   区位管理'} {...respProps}>
      {areaListReq.data && (
        <ProTable<WmsApi.WareArea>
          size="small"
          search={false}
          pagination={false}
          toolbar={{
            actions: [
              <AccessButton
                size="small"
                type="primary"
                func_code={FuncCodes.WMS_WareArea_Manage}
                onClick={() => {
                  setEditArea({ id: '0' } as any);
                }}
              >
                添加区位
              </AccessButton>,
            ],
          }}
          dataSource={areaListReq.data.data}
          columns={areaTableColumns}
        ></ProTable>
      )}

      <AreaEditor
        record={editArea}
        visible={editArea.id != '-1'}
        onCancel={() => {
          setEditArea(defaultArea);
        }}
        warehouse={warehouse}
        callback={(res) => {
          if (res.success) {
            setEditArea(defaultArea);
            areaListReq.run(warehouse.id);
          }
        }}
      ></AreaEditor>
    </Drawer>
  );
}
