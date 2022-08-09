import { AdminCreate } from '@/services/portal/admin_api';
import { useRequest } from 'ahooks';
import { Button, Drawer, Form, Input } from 'antd';
import { DrawerProps } from 'antd/lib/drawer';
import UserSelectModal from '../../user/components/user_select_modal';

interface AddAdminProps extends DrawerProps {
  callback: (res: IResp) => void;
}

const AddAdmin = (props: AddAdminProps) => {
  const { callback, ...restProps } = props;

  const createReq = useRequest(AdminCreate, {
    manual: true,
    onSuccess(res) {
      callback(res);
    },
  });

  async function onFinish(vals: any) {
    createReq.run(vals);
  }

  return (
    <Drawer placement="right" title="创建管理员" width={640} {...restProps}>
      <Form name="add_admin" onFinish={onFinish}>
        <Form.Item label="选择关联用户：" name="id">
          <UserSelectModal></UserSelectModal>
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
