import React, { useRef, useState } from 'react';
import BodyContent from '@/layouts/compents/body_content';

import { Avatar, message } from 'antd';
import { formatTimestamp } from '@/utils/utils';
import { searchAdmins, lockAdmin, setAdminType } from './service';
import { AdminInfo } from './data_d';
import { PlusOutlined, LockOutlined, UnlockOutlined } from '@ant-design/icons';
import AddAdmin from './compents/add_admin';
import { FuncCodes } from '@/utils/resp_d';
import { FormItemFactoryType, FormItemFactoryProps } from '@/components/form/form_item_factory';
import SearchTable, {
  getTextFromTableStatus,
  SearchTableAction,
} from '@/components/search/search_table';
import SearchForm from '@/components/search/search_form';
import TableFetchButtons from '@/components/button/table_Fetch_buttons';
import AccessButton from '@/components/button/access_button';
const AdminList: React.FC<{}> = () => {
  const tableRef = useRef<SearchTableAction>();
  // const [formRef] = Form.useForm();
  const [addAdminVis, setAddAdminVis] = useState(false);

  const searchFormItems: FormItemFactoryProps[] = [
    {
      type: FormItemFactoryType.input,
      label: '名称',
      name: 'admin_name',
    },
  ];
  const tableStatus = [
    { label: '正常', value: '0' },
    { label: '待绑定', value: '-20' },
    { label: '已锁定', value: '-100' },
    { label: '全部', value: '-999' },
  ];

  const statusButtons = [
    {
      condition: (r: AdminInfo) => r.status >= -20,
      buttons: [
        {
          btn_text: '锁定',
          fetch: (item: AdminInfo) => lockAdmin(item, true),
          fetch_desp: '锁定当前管理员',
          func_code: FuncCodes.Portal_AdminLock,
          icon: <LockOutlined />,
        },
      ],
    },
    {
      condition: (r: AdminInfo) => r.status == -100,
      buttons: [
        {
          btn_text: '解锁',
          fetch: (item: AdminInfo) => lockAdmin(item, false),
          fetch_desp: '解锁当前管理员',
          func_code: FuncCodes.Portal_AdminUnLock,
          icon: <UnlockOutlined />,
        },
      ],
    },
    {
      condition: (r: AdminInfo) => r.admin_type == 100,
      buttons: [
        {
          btn_text: '取消超管权限',
          fetch: (item: AdminInfo) => setAdminType(item, 0),
          fetch_desp: '将超级管理员降为普通管理员',
          func_code: FuncCodes.Portal_AdminSetType,
          // icon: <settt />,
        },
      ],
    },
    {
      condition: (r: AdminInfo) => r.admin_type == 0,
      buttons: [
        {
          btn_text: '设置超管权限',
          fetch: (item: AdminInfo) => setAdminType(item, 100),
          fetch_desp: '设置当前管理员为超级管理员',
          func_code: FuncCodes.Portal_AdminSetType,
          // icon: <settt />,
        },
      ],
    },
  ];

  const tableColumns = [
    {
      title: '头像',
      hideInSearch: true,
      dataIndex: 'avatar',
      render: (_: any, record: AdminInfo) => (
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
      title: '创建时间',
      dataIndex: 'add_time',
      render: (_: any, record: AdminInfo) => formatTimestamp(record.add_time),
    },
    {
      title: '状态',
      dataIndex: 'status',
      render: (v: any, _: any) => getTextFromTableStatus(v, tableStatus),
    },
    {
      title: '操作',
      dataIndex: 'id',
      render: (_: any, r: AdminInfo) => (
        <TableFetchButtons
          record={r}
          callback={(res, item, aName) => {
            if (res.is_ok) tableRef.current?.refresh();
          }}
          fetchKey={(item) => item.id}
          condition_buttons={statusButtons}
        ></TableFetchButtons>
      ),
    },
  ];

  const defaultFilter = { status: '0' };

  return (
    <BodyContent>
      <SearchForm
        items={searchFormItems}
        // form={formRef}
        initialValues={defaultFilter}
        onFinish={(vals) => {
          tableRef.current?.reload(vals);
        }}
        top_radios={{
          name: 'status',
          options: tableStatus,
        }}
      >
           <AccessButton
            type="primary"
            onClick={() => {
              setAddAdminVis(true);
            }}
            func_code={FuncCodes.Portal_AdminCreate}
          >
              <PlusOutlined />
            新增管理员
          </AccessButton>
        
      </SearchForm>

      <SearchTable<AdminInfo>
        rowKey="id"
        columns={tableColumns}
        search_table_ref={tableRef}
        search_fetch={searchAdmins}
        default_filters={defaultFilter}
      />

      <AddAdmin
        visible={addAdminVis}
        onClose={() => {
          setAddAdminVis(false);
        }}
        callback={(res) => {
          if (res.is_failed) {
            message.error(res.msg);
            return;
          }
          setAddAdminVis(false);
          tableRef.current?.refresh(true);
        }}
      />
    </BodyContent>
  );
};

export default AdminList;
