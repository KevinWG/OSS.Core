import React from 'react';
import { useRequest } from 'umi';
import { Form, Input, Button, Checkbox } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';

import styles from '../style.less';
import { adminUserPasswordLogin, getLoginType, AdminIdentity } from '../service';
import { RespData } from '@/utils/resp_d';

export default function PasswordLoginForm(props: {
  call_back: (res: RespData<AdminIdentity>) => void;
}) {
  const loginReq = useRequest(adminUserPasswordLogin, {
    manual: true,
  });
  const onFinish = async (values: any) => {
    values.type = getLoginType(values.name);
    loginReq.run(values).then((res) => {
      props.call_back(res);
    });
  };

  const iconStyle = { color: '#1890ff' };

  return (
    <Form
      name="password_login"
      initialValues={{ remember: true }}
      scrollToFirstError={true}
      onFinish={onFinish}
      size="large"
    >
      <Form.Item
        name="name"
        validateFirst={true}
        rules={[
          { required: true, message: '手机号或邮箱 不能为空!' },
          {
            validator(_, value) {
              if (getLoginType(value)) {
                return Promise.resolve();
              }
              return Promise.reject('请输入正确的手机号或邮箱!');
            },
          },
        ]}
      >
        <Input prefix={<UserOutlined style={iconStyle} />} placeholder="请输入 手机号或邮箱" />
      </Form.Item>

      <Form.Item
        name="password"
        rules={[{ required: true, message: '密码不能少于六位!', type: 'string', min: 6 }]}
      >
        <Input.Password prefix={<LockOutlined style={iconStyle} />} placeholder="请输入 密码" />
      </Form.Item>

      <Form.Item name="remember" valuePropName="checked">
        <Checkbox>自动登录</Checkbox>
      </Form.Item>

      <Form.Item>
        <Button
          type="primary"
          htmlType="submit"
          loading={loginReq.loading}
          className={styles.submit}
          size="large"
        >
          登录
        </Button>
      </Form.Item>
    </Form>
  );
}
