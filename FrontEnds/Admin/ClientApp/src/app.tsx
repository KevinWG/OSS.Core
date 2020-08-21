import { history } from 'umi';

import defaultSettings, { DefaultSettings } from '../config/defaultSettings';
import requestConfig from '../config/config_request';
import layoutConfig from './layouts/layout';
import { AdminIdentity, getAuthAdminWithAccess } from './pages/portal/login/service';
import { PortalConfig } from '../config/config_routes';

export async function getInitialState(): Promise<{
  currentUser?: AdminIdentity;
  settings?: DefaultSettings;
}> {
  // 如果是登录页面，不执行
  if (history.location.pathname !== PortalConfig.login_url) {
    try {
      const userRes = await getAuthAdminWithAccess();
      if (userRes.is_ok) {
        return {
          currentUser: userRes.data,
          settings: defaultSettings,
        };
      }
    } catch (error) {}
  }
  return {
    settings: defaultSettings,
  };
}

export const layout = layoutConfig;
export const request = requestConfig;
