import { LockOutlined, UnlockOutlined } from '@ant-design/icons';
import { ActionType, PageContainer, ProColumns } from '@ant-design/pro-components';
import { Avatar } from 'antd';

import moment from 'moment';
import React, { useRef, useState } from 'react';

import TableFetchButtons from '@/components/button/table_Fetch_buttons';
import SearchProTable from '@/components/search/search_protable';

import { lockUser, searchUsers } from '@/pages/portal/user/service';
import { UserStatus } from '@/services/portal/enums';

import AccessButton from '@/components/button/access_button';
import FuncCodes from '@/services/common/func_codes';
import AddUser from './components/add_user';
import Profile from './components/Profile';

const statusButtons = [
  {
    condition: (r: PortalApi.UserInfo) => r.status == 0,
    buttons: [
      {
        btn_text: '锁定',
        fetch: (item: PortalApi.UserInfo) => lockUser(item.id, true),
        fetch_desp: '锁定当前用户',
        func_code: FuncCodes.portal_user_lock,
        icon: <LockOutlined />,
      },
    ],
  },
  {
    condition: (r: PortalApi.UserInfo) => r.status == -100,
    buttons: [
      {
        btn_text: '解锁',
        fetch: (item: PortalApi.UserInfo) => lockUser(item.id, false),
        fetch_desp: '解锁当前用户',
        func_code: FuncCodes.portal_user_unlock,
        icon: <UnlockOutlined />,
      },
    ],
  },
];

const tableSegmentCols: ProColumns<PortalApi.UserInfo>[] = [
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
    valueType: 'dateRange',
    render: (_, e) => moment(e.add_time * 1000).format('YYYY-MM-DD'),
    search: {
      transform: (value) => {
        const dateRange = {
          reg_start_at: Math.ceil(value[0] / 1000),
          reg_end_at: Math.ceil(value[1] / 1000),
        };
        return dateRange;
      },
    },
  },
  {
    title: '状态',
    dataIndex: 'status',
    valueType: 'select',
    valueEnum: UserStatus,
  },
];

const UserList: React.FC<{}> = () => {
  const tableAction = useRef<ActionType>();

  const tableColumns: ProColumns<PortalApi.UserInfo>[] = [
    {
      title: '头像',
      hideInSearch: true,
      dataIndex: 'avatar',
      render: (_: any, record: PortalApi.UserInfo) => (
        <a onClick={() => setUserInfo(record)}>
          <Avatar src={record.avatar} />
        </a>
      ),
    },
    ...tableSegmentCols,
    {
      title: '操作',
      dataIndex: 'id',
      hideInSearch: true,
      render: (_: any, r: PortalApi.UserInfo) => (
        <TableFetchButtons
          record={r}
          callback={(res, item, aName) => {
            if (res.success) tableAction.current?.reload();
          }}
          fetchKey={(item) => item.id}
          condition_buttons={statusButtons}
        ></TableFetchButtons>
      ),
    },
  ];

  const defaultUserInfo = { id: '' } as PortalApi.UserInfo;
  const [userInfo, setUserInfo] = useState(defaultUserInfo);

  return (
    <PageContainer>
      <SearchProTable<PortalApi.UserInfo>
        rowKey="id"
        columns={tableColumns}
        request={searchUsers}
        actionRef={tableAction}
        dateFormatter="number"
        toolbar={{
          actions: [
            <AccessButton
              type="primary"
              func_code={FuncCodes.portal_user_add}
              onClick={() => {
                setUserInfo({ id: '0' } as any);
              }}
            >
              添加用户
            </AccessButton>,
          ],
        }}
      />
      <Profile
        visible={parseInt(userInfo.id) > 0}
        user={userInfo}
        onClose={() => {
          setUserInfo(defaultUserInfo);
        }}
      ></Profile>
      <AddUser
        visible={userInfo.id == '0'}
        callback={(res) => {
          if (res.success) {
            setUserInfo(defaultUserInfo);

            if (tableAction.current?.reloadAndRest) {
              tableAction.current?.reloadAndRest();
            } else {
              tableAction.current?.reload(true);
            }
          }
        }}
        onClose={() => {
          setUserInfo(defaultUserInfo);
        }}
      ></AddUser>
    </PageContainer>
  );
};

export default UserList;
