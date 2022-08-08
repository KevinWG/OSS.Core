import { login_path } from '@/components/layout/layout_const';
import { AxiosResponse, history, RequestConfig } from '@umijs/max';
import { message } from 'antd';
import { RespCodes } from './enums';

function requUrlChanger(reqOpt: any) {
  reqOpt.url = '/api'.concat(reqOpt.url); //.concat('?token = 123');
  return reqOpt;
}

function responseStatus<T = any>(response: AxiosResponse<T>): AxiosResponse<T> {
  const { data } = response;

  const res: any = data || { code: 500 };
  res.success = !res.code;

  if (!res.success) {
    if (res.code == RespCodes.UserUnLogin) {
      // 未登录，全局错误且跳转至登录页
      history.push(login_path);
    } else {
      message.error(res.msg);
    }
  } else if (res.msg) {
    console.info(res.msg);
  }

  return !data
    ? {
        ...response,
        data: res,
      }
    : response;
}

function errorHandler(error: any, opts: any) {
  if (opts?.skipErrorHandler) throw error;
  message.error('请求出现异常，请稍后重试!');
}

const default_request_config: RequestConfig = {
  headers: {
    'x-core-app': 'admin_client',
    'Content-Type': 'application/json',
  },

  transformResponse: [
    function (data) {
      data = data.replace(/"(\s*):(\s*)(\d{15,})/g, '":"$3"');
      return JSON.parse(data);
    },
  ],
  errorConfig: {
    errorHandler: errorHandler,
  },
  requestInterceptors: [requUrlChanger],
  responseInterceptors: [responseStatus],
};

export default default_request_config;
