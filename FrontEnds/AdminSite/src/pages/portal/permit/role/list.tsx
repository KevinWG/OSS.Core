import React, { useRef, useState } from 'react';

import AccessButton from '@/components/button/access_button';
import TableButton from '@/components/button/table_button';
import TableFetchButtons from '@/components/button/table_Fetch_buttons';
import SearchProTable from '@/components/search/search_protable';
import { activeRole, searchRoles, unactiveRole } from '@/pages/portal/permit/role/service';
import { CommonStatus } from '@/services/common/enums';
import FuncCodes from '@/services/common/func_codes';
import {
  CheckCircleOutlined,
  CloseCircleOutlined,
  EditOutlined,
  SearchOutlined,
} from '@ant-design/icons';
import { ActionType, PageContainer, ProColumns } from '@ant-design/pro-components';

import moment from 'moment';
import RoleEditor from './role_editor';
import EditRoleFunc from './role_funcs';

const statusButtons = [
  {
    condition: (r: PortalApi.RoleMo) => r.status == 0,
    buttons: [
      {
        btn_text: '锁定',
        fetch: (item: PortalApi.RoleMo) => unactiveRole(item.id),
        fetch_desp: '锁定角色',
        func_code: FuncCodes.portal_role_active,
        icon: <CloseCircleOutlined />,
      },
    ],
  },
  {
    condition: (r: PortalApi.RoleMo) => r.status == -100,
    buttons: [
      {
        btn_text: '激活',
        fetch: (item: PortalApi.RoleMo) => activeRole(item.id),
        fetch_desp: '激活角色',
        func_code: FuncCodes.portal_role_active,
        icon: <CheckCircleOutlined />,
      },
    ],
  },
];

const RoleList: React.FC<{}> = () => {
  const tableRef = useRef<ActionType>();

  const tableColumns: ProColumns<PortalApi.RoleMo>[] = [
    {
      title: '名称',
      dataIndex: 'name',
    },
    {
      title: '状态',
      dataIndex: 'status',
      valueType: 'select',
      valueEnum: CommonStatus,
      initialValue: '0',
    },
    {
      title: '创建时间',
      dataIndex: 'add_time',
      hideInSearch: true,
      render: (_, e) => moment(e.add_time * 1000).format('YYYY-MM-DD'),
    },
    {
      title: '关联权限',
      dataIndex: 'id',
      render: (_: any, record: any) => (
        <TableButton
          onClick={() => {
            setFuncRole(record);
          }}
          func_code={FuncCodes.portal_grant_role_permits}
          icon={<SearchOutlined />}
        >
          查看关联权限
        </TableButton>
      ),
    },
    {
      title: '操作',
      dataIndex: 'id',
      hideInSearch: true,
      render: (_: any, r: PortalApi.RoleMo) => (
        <>
          <TableButton
            func_code={FuncCodes.portal_role_add}
            hidden={r.status != 0}
            onClick={() => {
              setEditRole(r);
            }}
            icon={<EditOutlined />}
          >
            编辑
          </TableButton>

          <TableFetchButtons
            record={r}
            callback={(res, item, aName) => {
              if (res.success) tableRef.current?.reload();
            }}
            fetchKey={(item) => item.id}
            condition_buttons={statusButtons}
          ></TableFetchButtons>
        </>
      ),
    },
  ];

  const defaultRole = {} as PortalApi.RoleMo;
  const [editRole, setEditRole] = useState(defaultRole);

  const [funcRole, setFuncRole] = useState(defaultRole);

  return (
    <PageContainer>
      <SearchProTable<PortalApi.RoleMo>
        rowKey="id"
        columns={tableColumns}
        actionRef={tableRef}
        request={searchRoles}
        toolbar={{
          actions: [
            <AccessButton
              type="primary"
              func_code={FuncCodes.portal_role_add}
              onClick={() => {
                setEditRole({ id: '0' } as any);
              }}
            >
              添加角色
            </AccessButton>,
          ],
        }}
      />
      <RoleEditor
        record={editRole}
        callback={(res) => {
          if (res.success) {
            tableRef.current?.reload();
            setEditRole(defaultRole);
          }
        }}
        visible={!!editRole.id}
        onCancel={() => {
          setEditRole(defaultRole);
        }}
      ></RoleEditor>

      <EditRoleFunc
        visible={!!funcRole.id}
        show_type={funcRole.id == '0' ? 'show_all' : 'show_role'}
        onClose={() => {
          setFuncRole(defaultRole);
        }}
        record={funcRole}
        callback={(res) => {
          setFuncRole(defaultRole);
        }}
      ></EditRoleFunc>
    </PageContainer>
  );
};

export default RoleList;
