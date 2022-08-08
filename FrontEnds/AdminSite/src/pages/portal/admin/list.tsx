import { Avatar } from 'antd';
import React, { useRef, useState } from 'react';

import { LockOutlined, UnlockOutlined } from '@ant-design/icons';

import TableFetchButtons from '@/components/button/table_Fetch_buttons';

import AccessButton from '@/components/button/access_button';
import SearchProTable from '@/components/search/search_protable';
import FuncCodes from '@/services/common/func_codes';

import { lockAdmin, searchAdmins, setAdminType } from '@/services/portal/admin_api';

import TableButton from '@/components/button/table_button';
import { AdminType, UserStatus } from '@/services/portal/enums';
import { ActionType, PageContainer, ProColumns } from '@ant-design/pro-components';
import moment from 'moment';
import AddAdmin from './components/add_admin';
import AdminDetail from './components/admin_detail';

const staticCols: ProColumns<PortalApi.AdminInfo>[] = [
  {
    title: '头像',
    hideInSearch: true,
    dataIndex: 'avatar',
    render: (_: any, record: PortalApi.AdminInfo) =>
      record.avatar && (
        <a>
          <Avatar src={record.avatar + '/s100'} size={60} />
        </a>
      ),
  },
  {
    title: '名称',
    dataIndex: 'admin_name',
  },
  {
    title: '类型',
    dataIndex: 'admin_type',
    valueType: 'select',
    valueEnum: AdminType,
  },
  {
    title: '状态',
    dataIndex: 'status',
    valueType: 'select',
    valueEnum: UserStatus,
  },
  {
    title: '创建时间',
    dataIndex: 'add_time',
    valueType: 'dateRange',

    render: (_, e) => moment(e.add_time * 1000).format('YYYY-MM-DD'),
    search: {
      transform: (value) => {
        const dateRange = {
          add_start_at: Math.ceil(value[0] / 1000),
          add_end_at: Math.ceil(value[1] / 1000),
        };
        console.info(dateRange);
        return dateRange;
      },
    },
  },
];

const statusButtons = [
  {
    condition: (r: PortalApi.AdminInfo) => r.status >= -20,
    buttons: [
      {
        btn_text: '锁定',
        fetch: (item: PortalApi.AdminInfo) => lockAdmin(item, true),
        fetch_desp: '锁定当前管理员',
        func_code: FuncCodes.portal_admin_lock,
        icon: <LockOutlined />,
      },
    ],
  },
  {
    condition: (r: PortalApi.AdminInfo) => r.status == -100,
    buttons: [
      {
        btn_text: '解锁',
        fetch: (item: PortalApi.AdminInfo) => lockAdmin(item, false),
        fetch_desp: '解锁当前管理员',
        func_code: FuncCodes.portal_admin_unlock,
        icon: <UnlockOutlined />,
      },
    ],
  },
  {
    condition: (r: PortalApi.AdminInfo) => r.admin_type == 100,
    buttons: [
      {
        btn_text: '取消超管权限',
        fetch: (item: PortalApi.AdminInfo) => setAdminType(item, 0),
        fetch_desp: '将超级管理员降为普通管理员',
        func_code: FuncCodes.portal_admin_settype,
      },
    ],
  },
  {
    condition: (r: PortalApi.AdminInfo) => r.admin_type == 0,
    buttons: [
      {
        btn_text: '设置超管权限',
        fetch: (item: PortalApi.AdminInfo) => setAdminType(item, 100),
        fetch_desp: '设置当前管理员为超级管理员',
        func_code: FuncCodes.portal_admin_settype,
      },
    ],
  },
];
const AdminList: React.FC<{}> = () => {
  const tableRef = useRef<ActionType>();
  const tableColumns: ProColumns<PortalApi.AdminInfo>[] = [
    ...staticCols,
    {
      title: '操作',
      dataIndex: 'id',
      hideInSearch: true,
      render: (_: any, r: PortalApi.AdminInfo) => (
        <TableFetchButtons
          record={r}
          callback={(res, item, aName) => {
            if (res.success) tableRef.current?.reload();
          }}
          fetchKey={(item) => item.id}
          condition_buttons={statusButtons}
        >
          <TableButton
            func_code={FuncCodes.portal_admin_list}
            onClick={() => {
              setAdminInfo(r);
            }}
          >
            详情
          </TableButton>
        </TableFetchButtons>
      ),
    },
  ];

  const defaultAdminInfo = { id: '' } as PortalApi.AdminInfo;
  const [adminInfo, setAdminInfo] = useState(defaultAdminInfo);

  return (
    <PageContainer>
      <SearchProTable<PortalApi.AdminInfo>
        rowKey="id"
        columns={tableColumns}
        actionRef={tableRef}
        request={searchAdmins}
        dateFormatter="number"
        toolbar={{
          actions: [
            <AccessButton
              type="primary"
              func_code={FuncCodes.portal_admin_create}
              onClick={() => {
                setAdminInfo({ id: '0' } as any);
              }}
            >
              添加管理员
            </AccessButton>,
          ],
        }}
      />
      <AddAdmin
        visible={adminInfo.id == '0'}
        onClose={() => {
          setAdminInfo(defaultAdminInfo);
        }}
        callback={(res) => {
          setAdminInfo(defaultAdminInfo);
          tableRef.current?.reload(true);
        }}
      />

      <AdminDetail
        admin_info={adminInfo}
        visible={!!adminInfo.id && adminInfo.id != '0'}
        onClose={() => {
          setAdminInfo(defaultAdminInfo);
        }}
      ></AdminDetail>
    </PageContainer>
  );
};

export default AdminList;
