import React, { useRef, useState } from 'react';
import { Form, Space } from 'antd';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import { CloseCircleOutlined } from '@ant-design/icons';

import SearchForm from '@/components/Search/search_form';
import SearchTable, {
  SearchTableAction,
  getTextFromTableStatus,
} from '@/components/Search/search_table';
import { FormItemFactoryProps, FormItemFactoryType } from '@/components/form/form_item_factory';
import TableAccessButtons from '@/components/Button/table_access_buttons';
import AccessButton from '@/components/Button/access_button';

import { formatTimestamp } from '@/utils/utils';

import { RoleUserInfo } from './data_d';
import { OperateRoleUserStatus, searchRoleUsers } from './service';
import BindRoleUser from './compents/bind_role_user';
import { FuncCodes } from '@/utils/resp_d';

const tableStatus = [
  { label: '正常', value: '0' },
  { label: '全部', value: '-999' },
];

const searchFormItems: FormItemFactoryProps[] = [
  {
    type: FormItemFactoryType.input,
    label: '角色名称',
    name: 'r_name',
  },
  {
    type: FormItemFactoryType.input,
    label: '管理员名称',
    name: 'u_name',
  },
];

const RoleList: React.FC<{}> = () => {
  const [formRef] = Form.useForm();
  const tableRef = useRef<SearchTableAction>();

  const [addBindShow, setAddBindShow] = useState(false);
  const defaultFilter = { status: '0' };

  const statusButtons = [
    {
      condition: (r: RoleUserInfo) => r.status == 0,
      buttons: [
        {
          btn_text: '取消绑定',
          fetch: (item: RoleUserInfo) => OperateRoleUserStatus('deleteRoleBind', item),
          fetch_desp: '取消用户角色绑定',
          func_code: FuncCodes.Permit_RoleUserDelete,
          icon: <CloseCircleOutlined />,
        },
      ],
    },
  ];

  const tableColumns = [
    {
      title: '角色名称',
      dataIndex: 'r_name',
    },
    {
      title: '管理员名称',
      dataIndex: 'u_name',
    },
    {
      title: '状态',
      dataIndex: 'status',
      render: (v: any, _: any) => getTextFromTableStatus(v, tableStatus),
    },
    {
      title: '创建时间',
      dataIndex: 'add_time',
      render: (t: any, _: any) => formatTimestamp(t),
    },
    {
      title: '操作',
      dataIndex: 'id',
      render: (_: any, r: RoleUserInfo) => (
        <TableAccessButtons
          record={r}
          callback={(res, item, aName) => {
            if (res.is_ok) tableRef.current?.refresh();
          }}
          fetchKey={(item) => item.id}
          condition_buttons={statusButtons}
        ></TableAccessButtons>
      ),
    },
  ];

  return (
    <PageHeaderWrapper>
      <SearchForm
        items={searchFormItems}
        form={formRef}
        initialValues={defaultFilter}
        onFinish={(vals) => {
          tableRef.current?.reload(vals);
        }}
        top_radios={{
          name: 'status',
          options: tableStatus,
        }}
      >
        <Space>
          <AccessButton
            func_code={FuncCodes.Permit_RoleUserBind}
            type="primary"
            onClick={() => {
              setAddBindShow(true);
            }}
          >
            新建用户角色绑定
          </AccessButton>
        </Space>
      </SearchForm>

      <SearchTable<RoleUserInfo>
        rowKey="id"
        columns={tableColumns}
        search_table_ref={tableRef}
        search_fetch={searchRoleUsers}
        default_filters={defaultFilter}
      />

      <BindRoleUser
        visible={addBindShow}
        onClose={() => {
          setAddBindShow(false);
        }}
        callback={(res) => {
          if (res.is_ok) {
            tableRef.current?.refresh(true);
          }
        }}
      ></BindRoleUser>
    </PageHeaderWrapper>
  );
};

export default RoleList;
