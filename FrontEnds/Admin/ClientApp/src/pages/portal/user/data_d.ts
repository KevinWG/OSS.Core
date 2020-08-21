import { BaseMo } from '@/utils/resp_d';

export interface UserInfo extends BaseMo {
  nick_name: string;
  avatar: string;
  email: string;
}

export const user_status = {
  [-999]: '全部',
  [-100]: {
    text: '锁定',
    status: 'Error',
  },
  [-20]: {
    text: '待绑定',
    status: 'Warning',
  },
  [0]: {
    text: '正常',
    status: 'Success',
  },
};
