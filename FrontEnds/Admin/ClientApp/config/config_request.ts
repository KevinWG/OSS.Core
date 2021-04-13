import { PortalConfig } from './config_routes';
import { message } from 'antd';
import { history, RequestConfig } from 'umi';

// http状态码
const codeMessage = {
  200: '服务器成功返回请求的数据。',
  201: '新建或修改数据成功。',
  202: '一个请求已经进入后台排队（异步任务）。',
  204: '删除数据成功。',
  400: '发出的请求有错误，服务器没有进行新建或修改数据的操作。',
  401: '用户没有权限（令牌、用户名、密码错误）。',
  403: '用户得到授权，但是访问是被禁止的。',
  404: '发出的请求针对的是不存在的记录，服务器没有进行操作。',
  405: '当前请求方法不存在。',
  406: '请求的格式不可得。',
  410: '请求的资源被永久删除，且不会再得到的。',
  422: '当创建一个对象时，发生一个验证错误。',
  500: '服务器发生错误，请检查服务器。',
  502: '网关错误。',
  503: '服务不可用，服务器暂时过载或维护。',
  504: '网关超时。',
};

const apiStatusMidleware = async function middlewareA(ctx: any, next: any) {
  await next();
  const res = ctx.res;

  res.success = !res.sys_ret || res.sys_ret == 0;
  res.is_ok = !res.ret || res.ret == 0;
  res.is_failed = !res.is_ok;

  if (res.ret == 1425) {
    // 未登录，全局错误且跳转至登录页
    history.push(PortalConfig.login_url);
    return;
  }
  if (res.is_failed || !res.success) {
    res.errorMessage = res.msg;
  }

  if (res.success && res.is_failed && res.msg && res.ret != 1404) {
    message.error(res.msg);
    return;
  }
};

const defaultRequestConfig: RequestConfig = {
  headers: {
    'x-core-app': 'admin_client',
    'Content-Type': 'application/json',
  },
  errorConfig: {
    adaptor: (resData, { res }) => {
      if (res.status != 200 && !resData.errorMessage) {
        const errorMessage = codeMessage[res.status] || res.statusText;
        return {
          ...resData,
          errorMessage,
        };
      }
      return resData;
    },
  },
  middlewares: [apiStatusMidleware],
};

export default defaultRequestConfig;
