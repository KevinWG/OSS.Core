import type { Settings as LayoutSettings } from '@ant-design/pro-components';
import { history } from '@umijs/max';
import defaultSettings from '../config/default_layout_settings';
import baseLayout from './components/layout/layout';
import { login_path } from './components/layout/layout_const';
import default_request_config from './services/common/http_request';
import { getUserIdentityAndAccess } from './services/portal/auth_api';

/**
 * @see  https://umijs.org/zh-CN/plugins/plugin-initial-state
 * */
export async function getInitialState(): Promise<{
  settings?: Partial<LayoutSettings>;
  currentUser?: PortalApi.UserIdentity;
  loading?: boolean;
  fetchUserInfo?: () => Promise<PortalApi.UserIdentity | undefined>;
}> {
  const fetchUserInfo = async () => {
    try {
      const msg = await getUserIdentityAndAccess();
      return msg.data;
    } catch (error) {
      history.push(login_path);
    }
    return undefined;
  };
  // 如果不是登录页面，执行
  const path = history.location.pathname;
  if (path !== login_path && !path.includes('/404')) {
    const currentUser = await fetchUserInfo();
    return {
      fetchUserInfo,
      currentUser,
      settings: defaultSettings,
    };
  }
  return {
    fetchUserInfo,
    settings: defaultSettings,
  };
}

// ProLayout 支持的api https://procomponents.ant.design/components/layout
export const layout = baseLayout;

// 网络请求的拦截
export const request = default_request_config;
