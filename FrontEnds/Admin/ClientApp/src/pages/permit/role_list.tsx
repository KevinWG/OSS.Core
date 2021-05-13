import React, { useRef, useState } from 'react';
import { Form, Space } from 'antd';
import BodyContent from '@/layouts/compents/body_content';

import SearchForm from '@/components/search/search_form';
import SearchTable, {
  SearchTableAction,
  getTextFromTableStatus,
} from '@/components/search/search_table';
import { FormItemFactoryProps, FormItemFactoryType } from '@/components/form/form_item_factory';

import { formatTimestamp } from '@/utils/utils';

import { RoleInfo } from './data_d';
import EditRole from './compents/edit_role';
import { searchRoles, OperateRoleStatus } from './service';
import TableFetchButtons from '@/components/button/table_Fetch_buttons';
import {
  CheckCircleOutlined,
  CloseCircleOutlined,
  DeleteOutlined,
  EditOutlined,
  SearchOutlined,
} from '@ant-design/icons';
import EditRoleFunc from './compents/edit_role_funcs';
import { FuncCodes } from '@/utils/resp_d';
import AccessButton from '@/components/button/access_button';

const RoleList: React.FC<{}> = () => {
  const searchFormItems: FormItemFactoryProps[] = [
    {
      type: FormItemFactoryType.input,
      label: '名称',
      name: 'name',
    },
  ];

  const tableStatus = [
    { label: '正常', value: '0' },
    { label: '已作废', value: '-100' },
    { label: '全部', value: '-999' },
  ];

  const [formRef] = Form.useForm();
  const tableRef = useRef<SearchTableAction>();

  const [editShow, setEditShow] = useState(false);
  const [roleInfo, setRoleInfo] = useState<RoleInfo>();

  const [editFuncShow, setEditFuncShow] = useState(false);
  const [editFuncRoleInfo, setEditFuncRoleInfo] = useState<RoleInfo>();

  const defaultFilter = { status: '0' };

  const statusButtons = [
    {
      condition: (r: RoleInfo) => r.status == 0,
      buttons: [
        {
          btn_text: '关闭',
          fetch: (item: RoleInfo) => OperateRoleStatus('active_off', item),
          fetch_desp: '关闭角色',
          func_code: FuncCodes.Permit_RoleActive,
          icon: <CloseCircleOutlined />,
        },

      ],
    },
    {
      condition: (r: RoleInfo) => r.status == -100,
      buttons: [
        {
          btn_text: '启用',
          fetch: (item: RoleInfo) => OperateRoleStatus('active', item),
          fetch_desp: '启用角色',
          func_code: FuncCodes.Permit_RoleActive,
          icon: <CheckCircleOutlined />,
        },
        {
          btn_text: '删除',
          fetch: (item: RoleInfo) => OperateRoleStatus('delete', item),
          fetch_desp: '删除角色【慎用】，将无法找回！',
          func_code: FuncCodes.Permit_RoleDelete,
          icon: <DeleteOutlined />,
        },
      ],
    },
  ];

  const tableColumns = [
    {
      title: '名称',
      dataIndex: 'name',
    },
    {
      title: '状态',
      dataIndex: 'status',
      render: (v: any, _: any) => getTextFromTableStatus(v, tableStatus),
    },
    {
      title: '创建时间',
      dataIndex: 'add_time',
      render: (_: any, record: any) => formatTimestamp(record.add_time),
    },
    {
      title: '关联权限',
      dataIndex: 'id',
      render: (_: any, record: any) => (
        <AccessButton
          onClick={() => {
            setEditFuncRoleInfo(record);
            setEditFuncShow(true);
          }}
          func_code={FuncCodes.Permit_RoleFuncList}
          icon={<SearchOutlined />}
        >查看关联权限</AccessButton>
      ),
    },
    {
      title: '操作',
      dataIndex: 'id',
      render: (_: any, r: RoleInfo) => (
        <>
          <AccessButton
            func_code={FuncCodes.Permit_RoleUpdate}
            hidden={r.status != 0}
            onClick={() => {
              setRoleInfo(r);
              setEditShow(true);
            }}
            icon={<EditOutlined />}
          >编辑</AccessButton>
          <TableFetchButtons
            record={r}
            callback={(res, item, aName) => {
              if (res.is_ok) tableRef.current?.refresh();
            }}
            fetchKey={(item) => item.id}
            condition_buttons={statusButtons}
          ></TableFetchButtons></>
      ),
    },
  ];

  return (
    <BodyContent>
      <SearchForm
        // size="small"
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
            type="primary"
            onClick={() => {
              setRoleInfo(undefined);
              setEditShow(true);
            }}
            func_code={FuncCodes.Permit_RoleAdd}
          >
            新增角色
          </AccessButton>

          <AccessButton
            func_code={FuncCodes.Permit_RoleFuncList}
            type="default"
            onClick={() => {
              setEditFuncRoleInfo(undefined);
              setEditFuncShow(true);
            }}
          >
            查看系统全部权限
          </AccessButton>
        </Space>
      </SearchForm>

      <SearchTable<RoleInfo>
        rowKey="id"
        columns={tableColumns}
        search_table_ref={tableRef}
        search_fetch={searchRoles}
        default_filters={defaultFilter}
      />

      <EditRole
        visible={editShow}
        onClose={() => {
          setEditShow(false);
        }}
        record={roleInfo}
        callback={(res) => {
          if (res.is_ok) {
            tableRef.current?.refresh();
          }
        }}
      ></EditRole>

      <EditRoleFunc
        visible={editFuncShow}
        show_type={editFuncRoleInfo ? 'show_role' : 'show_all'}
        onClose={() => {
          setEditFuncShow(false);
        }}
        record={editFuncRoleInfo}
        callback={(res) => {
          setEditFuncShow(false);
        }}
      ></EditRoleFunc>
    </BodyContent>
  );
};

export default RoleList;
