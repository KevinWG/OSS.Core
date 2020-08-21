import { Form, Input, Button, Modal, Avatar, Table, message } from 'antd';
import React, { useState } from 'react';
import { ModalProps } from 'antd/lib/modal';
import { searchUsers } from '../service';
import { useRequest } from 'umi';
import { UserInfo } from '../data_d';

const AdvancedSearchForm = ({ onSearching }: any) => {
  const [form] = Form.useForm();

  const onFinish = (values: any) => {
    onSearching(values);
  };

  return (
    <div style={{ marginTop: 15, marginBottom: 20 }}>
      <Form form={form} name="advanced_search" onFinish={onFinish} layout="inline">
        <Form.Item label="手机号：" name="mobile">
          <Input placeholder="输入手机号" />
        </Form.Item>
        <Form.Item label="邮箱：" name="email">
          <Input placeholder="输入邮箱" />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit">
            搜索
          </Button>
        </Form.Item>
      </Form>
    </div>
  );
};

interface UserSelectModalProps extends ModalProps {
  onSelectUser: (u: UserInfo) => void;
}

const UserSelectModal = (props: UserSelectModalProps) => {
  const columns = [
    {
      key: 'avatar',
      title: '头像',
      dataIndex: 'avatar',
      render: (_: any, record: any) => <Avatar src={record.avatar} />,
    },
    {
      title: '昵称',
      dataIndex: 'nick_name',
      key: 'nick_name',
    },
    {
      title: '选择',
      dataIndex: 'operate',
      render: (_: any, record: any) => (
        <Button
          type="primary"
          onClick={() => {
            props.onSelectUser(record);
          }}
        >
          选择
        </Button>
      ),
    },
  ];

  const [userList, setUserList] = useState([] as UserInfo[]);
  const [total, setTotal] = useState(0);
  const defaultPageSize = 10;

  const [searchParas, setSearchParas] = useState({
    current: 1,
    pageSize: defaultPageSize,
  });

  const userSearchReq = useRequest(
    async (paras) => {
      var userListRes = await searchUsers(paras);
      if (userListRes.is_failed) {
        message.error(userListRes.msg);
        return;
      }
      setTotal(userListRes.total || 0);
      setUserList(userListRes.data);
      setSearchParas(paras);
    },
    {
      manual: true,
    },
  );

  function onSearching(filterVals: any) {
    var paras = { ...searchParas, ...filterVals, current: 1 };
    userSearchReq.run(paras);
  }

  function onPageChange(page: number, pageSize?: number) {
    var paras = { ...searchParas, current: page, pageSize: pageSize || defaultPageSize };
    userSearchReq.run(paras);
  }

  return (
    <Modal width={650} {...props} key="user_select_modal">
      <AdvancedSearchForm onSearching={onSearching} />
      <div>
        <Table
          key="user_select_list"
          rowKey="id"
          size="small"
          bordered
          columns={columns}
          dataSource={userList}
          pagination={{
            onChange: onPageChange,
            pageSize: defaultPageSize,
            current: searchParas.current,
            total: total,
          }}
        />
      </div>
    </Modal>
  );
};

export default UserSelectModal;
