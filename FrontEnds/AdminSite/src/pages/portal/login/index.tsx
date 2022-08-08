import Footer from '@/components/layout/Footer';
import {
  getLoginNameType,
  postCodeAdminLogin,
  postPwdAdminLogin,
  postSendCode,
} from '@/services/portal/auth_api';
import { PortalNameType } from '@/services/portal/enums';
import { LockOutlined, UserOutlined } from '@ant-design/icons';
import {
  LoginForm,
  ProFormCaptcha,
  ProFormCheckbox,
  ProFormText,
} from '@ant-design/pro-components';
import { FormattedMessage, history, useModel } from '@umijs/max';
import { Alert, message, Tabs } from 'antd';
import React, { useState } from 'react';
import styles from './index.less';

const LoginMessage: React.FC<{
  content: string;
}> = ({ content }) => {
  return (
    <Alert
      style={{
        marginBottom: 24,
      }}
      message={content}
      type="error"
      showIcon
    />
  );
};

const MobileOrEmailInput = (props: any) => (
  <ProFormText
    {...props}
    name="name"
    fieldProps={{
      size: 'large',
      prefix: <UserOutlined className={styles.prefixIcon} />,
    }}
    placeholder="请输入手机号或邮箱!"
    rules={[
      { required: true, message: '请输入手机号或邮箱!' },
      {
        validator(_, value) {
          if (getLoginNameType(value)) {
            return Promise.resolve();
          }
          return Promise.reject('请输入正确的手机号或邮箱!');
        },
      },
    ]}
  />
);

const Login: React.FC = () => {
  const [userLoginRes, setUserLoginRes] = useState({} as PortalApi.AuthResp);
  const [portalType, setPortalType] = useState('0');

  const { initialState, setInitialState } = useModel('@@initialState');

  const updateUserInfo = async () => {
    const userInfo = await initialState?.fetchUserInfo?.();

    if (userInfo) {
      await setInitialState((s) => ({
        ...s,
        currentUser: userInfo,
      }));
    }
  };

  const handleSubmit = async (values: any) => {
    // 登录
    const loginNameType = getLoginNameType(values.name);
    if (!loginNameType) {
      message.error('账号类型不正确!');
      return;
    }

    values.type = loginNameType as PortalNameType;

    const res = await (portalType == '0' ? postPwdAdminLogin(values) : postCodeAdminLogin(values));
    setUserLoginRes(res);

    if (res.success) {
      await updateUserInfo();

      const urlParams = new URL(window.location.href).searchParams;
      history.push(urlParams.get('rurl') || '/');
    }
  };

  return (
    <div className={styles.container}>
      {/* <div className={styles.lang} data-lang>
        {SelectLang && <SelectLang />}
      </div> */}
      <div className={styles.content}>
        <LoginForm
          logo={<img alt="logo" src="/logo.svg" />}
          title="开源OSSCore"
          subTitle=" "
          initialValues={{
            remember: true,
          }}
          actions={
            [
              // '其他登录方式',
              // <AlipayCircleOutlined key="AlipayCircleOutlined" className={styles.icon} />,
              // <TaobaoCircleOutlined key="TaobaoCircleOutlined" className={styles.icon} />,
              // <WeiboCircleOutlined key="WeiboCircleOutlined" className={styles.icon} />,
            ]
          }
          onFinish={async (values) => {
            await handleSubmit(values as PortalApi.PasswordAuthReq);
          }}
        >
          <Tabs activeKey={portalType} onChange={setPortalType}>
            <Tabs.TabPane key="0" tab="账户密码登录" />
            <Tabs.TabPane key="1" tab="动态码登录" />
          </Tabs>

          {portalType === '0' && (
            <>
              {userLoginRes.msg && <LoginMessage content={userLoginRes.msg} />}
              <MobileOrEmailInput />
              <ProFormText.Password
                name="password"
                fieldProps={{
                  size: 'large',
                  prefix: <LockOutlined className={styles.prefixIcon} />,
                }}
                placeholder="请输入密码"
                rules={[
                  {
                    required: true,
                    message: (
                      <FormattedMessage
                        id="pages.login.password.required"
                        defaultMessage="请输入密码！"
                      />
                    ),
                  },
                ]}
              />
            </>
          )}

          {portalType === '1' && (
            <>
              {userLoginRes.msg && <LoginMessage content={userLoginRes.msg} />}
              <MobileOrEmailInput />
              <ProFormCaptcha
                fieldProps={{
                  size: 'large',
                  prefix: <LockOutlined className={styles.prefixIcon} />,
                }}
                captchaProps={{
                  size: 'large',
                }}
                placeholder="请输入验证码"
                captchaTextRender={(timing, count) =>
                  timing ? `成功发送(${count})` : '获取验证码'
                }
                name="code"
                phoneName="name"
                rules={[
                  {
                    required: true,
                    message: '请输入验证码！',
                    type: 'number',
                    transform(value) {
                      if (value) {
                        return Number(value);
                      }
                      return undefined;
                    },
                  },
                  // { type: 'number', message: '验证码由数字组成!' },
                ]}
                onGetCaptcha={async (name) => {
                  const loginType = getLoginNameType(name);
                  const res = await postSendCode({
                    name,
                    type: loginType,
                  });
                  if (!res.success) throw res.msg;
                }}
              />
            </>
          )}
          <div
            style={{
              marginBottom: 24,
            }}
          >
            <ProFormCheckbox noStyle name="remember">
              自动登录
            </ProFormCheckbox>
            {/* <a
              style={{
                float: 'right',
              }}
            >
              忘记密码
            </a> */}
          </div>
        </LoginForm>
      </div>
      <Footer />
    </div>
  );
};

export default Login;
