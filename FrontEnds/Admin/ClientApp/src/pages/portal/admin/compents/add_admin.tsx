import React, { useState } from 'react';
import { Drawer, Form, Input, Button, Avatar, message } from 'antd';
import UserSelectModal from '@/pages/portal/user/components/user_select_modal';
import { UserInfo } from '../../user/data_d';
import { AdminCreate } from '../service';
import { useRequest } from 'umi';
import { AdminCreateReq } from '../data_d';
import { Resp } from '@/utils/resp_d';
import { DrawerProps } from 'antd/lib/drawer';

interface AddAdminProps extends DrawerProps {
  callback: (res: Resp) => void;
}

const AddAdmin = (props: AddAdminProps) => {
  const [userListVisible, setuserListVisible] = useState(false);
  const [userInfo, setUserInfo] = useState({} as UserInfo);
  const createReq = useRequest(AdminCreate, {
    manual: true,
  });

  const {callback,...restProps} = props;

  async function onFinish(vals: any) {
    if (!userInfo.id) {
      message.error('必须选择绑定用户！');
      return;
    }

    const reqBody = { u_id: userInfo.id, admin_name: vals.admin_name } as AdminCreateReq;
    const reqRes = await createReq.run(reqBody);

    callback(reqRes);
  }

  return (
    <Drawer placement="right" title="创建管理员" width={640} {...restProps}>
      <Form name="add_admin" onFinish={onFinish}>
        <Form.Item label="选择关联用户：">
          <Button
            type="primary"
            onClick={() => {
              setuserListVisible(true);
            }}
          >
            选择
          </Button>

          {userInfo.id && (
            <div style={{ marginTop: 15, textAlign: 'center' }}>
              <Avatar size={100} src={userInfo.avatar + '/s100'} />

              <div style={{ marginTop: 10 }}>{userInfo.nick_name}</div>
            </div>
          )}
          <UserSelectModal
            footer={null}
            onSelectUser={(u) => {
              setUserInfo(u);
              setuserListVisible(false);
            }}
            visible={userListVisible}
            onCancel={() => {
              setuserListVisible(false);
            }}
          />
        </Form.Item>
        <Form.Item name="admin_name" label="管理员名称" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit" loading={createReq.loading}>
            添加
          </Button>
        </Form.Item>
      </Form>
    </Drawer>
  );
};

export default AddAdmin;
