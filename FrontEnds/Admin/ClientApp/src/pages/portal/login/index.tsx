import React, { useState } from 'react';
import { Link, useModel } from 'umi';

import { Alert, Tabs } from 'antd';
import { RespData, Resp } from '@/utils/resp_d';
import { getPageQuery } from '@/utils/utils';
const { TabPane } = Tabs;

import PasswordLoginForm from './compents/password_login_form';
import CodeLoginForm from './compents/code_login_form';

import styles from './style.less';
import { AdminIdentity } from './service';

export default (props: any) => {
  // const { initialState, setInitialState } = useModel('@@initialState');
  const [errMsg, setErrMsg] = useState<Resp>();
  function callBack(res: RespData<AdminIdentity>) {
    if (res.is_ok) {
      // setInitialState({ ...initialState, currentUser: res.data });
      const query = getPageQuery();
      let { rurl } = query as { rurl: string };
      if (!rurl) {
        rurl = '/';
      }
      window.location.href = rurl; // 因为使用history，ProLayout菜单没有刷新是空，需要改写，改写前使用浏览器直接跳转
    } else {
      setErrMsg(res);
    }
  }
  return (
    <div className={styles.container}>
      <div className={styles.content}>
        <div className={styles.top}>
          <div className={styles.header}>
            <Link to="/">
              {/* <img alt="logo" className={styles.logo} src={require('@/assets/logo.png')} /> */}
              <span className={styles.title}>OSSCore</span>
            </Link>
          </div>
          <div className={styles.desc}>.Net Core 开源框架</div>
        </div>
        <div className={styles.main}>
          {errMsg?.is_failed && (
            <Alert
              style={{
                marginBottom: 24,
              }}
              message={errMsg?.msg || '网络请求出现问题！'}
              type="error"
              showIcon
            />
          )}

          <Tabs centered>
            <TabPane tab="账号密码登录" key="1">
              <PasswordLoginForm call_back={callBack}></PasswordLoginForm>
            </TabPane>
            <TabPane tab="动态码登录" key="2">
              <CodeLoginForm call_back={callBack}></CodeLoginForm>
            </TabPane>
          </Tabs>
        </div>
      </div>
    </div>
  );
};
