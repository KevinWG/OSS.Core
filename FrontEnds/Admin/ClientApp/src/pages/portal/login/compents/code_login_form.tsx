import React, { useState } from 'react';
import { useInterval, useCounter } from 'ahooks';
import { useRequest } from 'umi';
import { Form, Input, Button, Checkbox } from 'antd';
import { UserOutlined, NumberOutlined } from '@ant-design/icons';

import styles from '../style.less';
import { adminUserCodeLogin, getLoginType, AdminIdentity, sendCode } from '../service';
import { RespData } from '@/utils/resp_d';

export default function CodeLoginForm(props: {
  call_back: (res: RespData<AdminIdentity>) => void;
}) {
  const loginReq = useRequest(adminUserCodeLogin, {
    manual: true,
  });
  const codeReq = useRequest(sendCode, {
    manual: true,
  });

  const [counterVal, counterFunc] = useCounter(60);
  const [timerInterval, setTimerInterval] = useState<number>();

  useInterval(
    () => {
      counterFunc.dec();
      if (counterVal <= 1) {
        setTimerInterval(undefined);
        counterFunc.reset();
      }
    },
    timerInterval,
    { immediate: true },
  );

  const onFinish = async (values: any) => {
    values.type = getLoginType(values.name);

    var res = await loginReq.run(values);
    props.call_back(res);
  };

  // 发送动态验证码
  function sendCodeClieck() {
    loginForm.validateFields(['name']).then((nVal) => {
      const type = getLoginType(nVal.name);
      codeReq.run(type, nVal.name).then((res) => {
        if (res.is_ok) {
          setTimerInterval(1000); // 设置间隔1秒的计时器
        }
      });
    });
  }

  const iconStyle = { color: '#1890ff' };
  const [loginForm] = Form.useForm();
  return (
    <Form
      name="code_login"
      form={loginForm}
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
      <Form.Item>
        <Input.Group compact>
          <Form.Item name="code" noStyle rules={[{ required: true, message: '动态码不能为空!' }]}>
            <Input
              prefix={<NumberOutlined style={iconStyle} />}
              placeholder="请输入 动态码"
              style={{ width: '68%' }}
            />
          </Form.Item>
          <Form.Item noStyle>
            <Button
              style={{ width: '32%' }}
              loading={codeReq.loading}
              disabled={(timerInterval ?? 0) > 0}
              onClick={() => {
                sendCodeClieck();
              }}
              type="default"
            >
              {(timerInterval ?? 0) > 0 ? counterVal : '获取动态码'}
            </Button>
          </Form.Item>
        </Input.Group>
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
