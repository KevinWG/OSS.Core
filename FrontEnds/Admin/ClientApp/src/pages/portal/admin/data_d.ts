import { BaseMo } from '@/utils/resp_d';

export interface AdminInfo extends BaseMo {
  admin_name: string;
  avatar: string;
  u_id: string;
  admin_type: number;
}

export interface AdminCreateReq {
  u_id: string;
  admin_name: string;
}

export const admin_status = {
  [-999]: '全部',
  [-100]: { text: '锁定', status: 'Error' },
  [-20]: {
    text: '待绑定',
    status: 'Warning',
  },
  [0]: {
    text: '正常',
    status: 'Success',
  },
};
