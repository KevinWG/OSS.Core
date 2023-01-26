import { LockOutlined, UnlockOutlined } from '@ant-design/icons';
import { ActionType, PageContainer, ProColumns } from '@ant-design/pro-components';
import { Avatar } from 'antd';

import moment from 'moment';
import React, { useRef, useState } from 'react';

import TableFetchButtons from '@/components/button/table_Fetch_buttons';
import SearchProTable from '@/components/search/search_protable';

import AccessButton from '@/components/button/access_button';
import FuncCodes from '@/services/common/func_codes';
import { CommonStatus } from '@/services/common/enums';
import { spu_search } from './service';

const statusButtons = [
  {
    condition: (r: PortalApi.UserInfo) => r.status == 0,
    buttons: [
      {
        btn_text: '锁定',
        icon: <LockOutlined />,
      },
    ],
  },
  {
    condition: (r: PortalApi.UserInfo) => r.status == -100,
    buttons: [
      {
        btn_text: '解锁',
        icon: <UnlockOutlined />,
      },
    ],
  },
];

const tableSegmentCols: ProColumns<PortalApi.UserInfo>[] = [
  {
    title: '标题',
    dataIndex: 'title'
  },
  {
    title: '添加时间',
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
    valueEnum: CommonStatus,
  },
];

const List: React.FC<{}> = () => {
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
          // condition_buttons={statusButtons}
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
        request={spu_search}
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
    </PageContainer>
  );
};

export default List;
