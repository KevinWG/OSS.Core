// https://umijs.org/config/
import { defineConfig } from 'umi';
import defaultSettings from './defaultSettings';
import proxy from './proxy';
import routes from './config_routes';

const { REACT_APP_ENV } = process.env;
export default defineConfig({
  hash: true,
  // 表示生成文件是否含有hash值
  antd: {},
  outputPath: '.dist',
  dva: {
    hmr: true,
  },
  layout: {
    name: 'OSSCore',
    locale: false,
    logo: '/logo.png',
  },
  locale: {
    // default zh-CN
    default: 'zh-CN',
    // default true, when it is true, will use `navigator.language` overwrite default
    antd: true,
    baseNavigator: true,
  },
  dynamicImport: {
    loading: '@/components/page_loading/index',
  },
  request: {
    dataField: '',
  },
  targets: {
    ie: 11,
  },
  // umi routes: https://umijs.org/docs/routing
  routes: routes,
  // Theme for antd: https://ant.design/docs/react/customize-theme-cn
  theme: {
    // ...darkTheme,
    'primary-color': defaultSettings.primaryColor,
  },
  ignoreMomentLocale: true,
  proxy: proxy[REACT_APP_ENV || 'dev'],
  manifest: {
    basePath: '/',
  },

  // publicPath:"http://www.a.com/"
});
