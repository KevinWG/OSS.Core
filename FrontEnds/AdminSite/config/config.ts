// https://umijs.org/config/
import { defineConfig } from '@umijs/max';
import defaultSettings from './default_layout_settings';
import proxy from './proxy';
import routes from './routes';

const { REACT_APP_ENV } = process.env;

export default defineConfig({
  hash: true,
  antd: {},
  initialState: {},
  model: {},
  layout: {
    // https://umijs.org/zh-CN/plugins/plugin-layout
    locale: false,
    siderWidth: 208,
    ...defaultSettings,
  },
  // https://umijs.org/zh-CN/plugins/plugin-locale
  locale: {
    // default zh-CN
    default: 'zh-CN',
    antd: true,
    // default true, when it is true, will use `navigator.language` overwrite default
    // baseNavigator: true,
  },

  targets: {
    // ie: 11,
  },

  // umi routes: https://umijs.org/docs/routing
  routes,
  access: {},
  // Theme for antd: https://ant.design/docs/react/customize-theme-cn
  theme: {
    // 如果不想要 configProvide 动态设置主题需要把这个设置为 default
    // 只有设置为 variable， 才能使用 configProvide 动态设置主色调
    // https://ant.design/docs/react/customize-theme-variable-cn
    'root-entry-name': 'variable',
  },
  ignoreMomentLocale: true,
  proxy: proxy[REACT_APP_ENV || 'dev'],
  manifest: {
    basePath: '/',
  },
  // Fast Refresh 热更新
  fastRefresh: true,
  presets: ['umi-presets-pro'],

  request: {
    dataField: '',
  },

  // openAPI: [
  //   {
  //     requestLibPath: "import { request } from '@umijs/max'",
  //     schemaPath: 'http://localhost:8043/swagger/v1/swagger.json',
  //     projectName: 'portal',
  //     apiPrefix: '/api/',
  //     namespace: 'PortalApi',
  //   },
  // ],
});
