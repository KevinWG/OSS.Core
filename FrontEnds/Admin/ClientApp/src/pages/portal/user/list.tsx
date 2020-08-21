import React from 'react';
import { Form, Avatar } from 'antd';
import { useRef, useState } from 'react';
import { FormItemFactoryType, FormItemFactoryProps } from '@/components/form/form_item_factory';
import SearchTable, {
  SearchTableAction,
  getTextFromTableStatus,
} from '@/components/Search/search_table';
import TableAccessButtons from '@/components/Button/table_access_buttons';
import { PageHeaderWrapper } from '@ant-design/pro-layout';
import SearchForm from '@/components/Search/search_form';

import { formatTimestamp } from '@/utils/utils';

import { FuncCodes, Resp } from '@/utils/resp_d';
import { UserInfo } from './data_d';
import { searchUsers, lockUser } from './service';
import { LockOutlined, UnlockOutlined } from '@ant-design/icons';
import Profile from './components/Profile';
import AccessButton from '@/components/Button/access_button';
import AddUser from './components/add_user';

const UserList: React.FC<{}> = () => {
  const searchFormItems: FormItemFactoryProps[] = [
    {
      type: FormItemFactoryType.input,
      label: '昵称',
      name: 'nick_name',
    },
    {
      type: FormItemFactoryType.input,
      label: '手机号',
      name: 'mobile',
    },
    {
      type: FormItemFactoryType.input,
      label: '邮箱',
      name: 'email',
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
      condition: (r: UserInfo) => r.status == 0,
      buttons: [
        {
          btn_text: '锁定',
          fetch: (item: UserInfo) => lockUser(item, true),
          fetch_desp: '锁定当前用户',
          func_code: FuncCodes.Portal_UserLock,
          icon: <LockOutlined />,
        },
      ],
    },
    {
      condition: (r: UserInfo) => r.status == -100,
      buttons: [
        {
          btn_text: '解锁',
          fetch: (item: UserInfo) => lockUser(item, false),
          fetch_desp: '解锁当前用户',
          func_code: FuncCodes.Portal_UserUnLock,
          icon: <UnlockOutlined />,
        },
      ],
    },
  ];

  const tableColumns = [
    {
      title: '头像',
      hideInSearch: true,
      dataIndex: 'avatar',
      render: (_, record: UserInfo) => (
        <a onClick={() => showUserProfile(record)}>
          <Avatar src={record.avatar} />
        </a>
      ),
    },
    {
      title: '昵称',
      dataIndex: 'nick_name',
      hideInSearch: true,
    },
    {
      title: '邮箱',
      dataIndex: 'email',
    },
    {
      title: '手机号',
      dataIndex: 'mobile',
    },
    {
      title: '注册时间',
      dataIndex: 'add_time',
      render: (_, record: UserInfo) => formatTimestamp(record.add_time),
    },

    {
      title: '状态',
      dataIndex: 'status',
      render: (v: any, _: any) => getTextFromTableStatus(v, tableStatus),
    },

    {
      title: '操作',
      dataIndex: 'id',
      render: (_: any, r: UserInfo) => (
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

  const [formRef] = Form.useForm();
  const tableRef = useRef<SearchTableAction>();

  const defaultFilter = { status: '0' };
  //  展示详情
  var [profileVisible, setProfileVisible] = useState(false);
  var [profileInfo, setProfileInfo] = useState({} as UserInfo);

  //  添加用户
  var [addUserVisible, setAddUserVisible] = useState(false);

  function showUserProfile(item: UserInfo) {
    setProfileInfo(item);
    setProfileVisible(true);
  }
  function closeUserProfile() {
    setProfileInfo({} as UserInfo);
    setProfileVisible(false);
  }

  function addUserCallBack(res: Resp) {
    tableRef.current?.refresh(true);
  }

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
        <AccessButton
          type="primary"
          func_code={FuncCodes.Portal_UserAdd}
          onClick={() => {
            setAddUserVisible(true);
          }}
        >
          新增用户
        </AccessButton>
      </SearchForm>

      <SearchTable<UserInfo>
        rowKey="id"
        columns={tableColumns}
        search_table_ref={tableRef}
        search_fetch={searchUsers}
        default_filters={defaultFilter}
      />
      <Profile visible={profileVisible} user={profileInfo} onClose={closeUserProfile}></Profile>
      <AddUser
        visible={addUserVisible}
        callback={addUserCallBack}
        onClose={() => {
          setAddUserVisible(false);
        }}
      ></AddUser>
    </PageHeaderWrapper>
  );
};

export default UserList;
