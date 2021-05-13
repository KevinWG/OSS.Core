import { BaseMo } from '@/utils/resp_d';

export interface RoleUserInfo extends RoleInfo {
  u_name: string;
  r_name: string;
  u_id: string;
  r_id: string;
}

export interface RoleInfo extends BaseMo {
  name: string;
}

export interface FuncItem {
  title: string;
  code: string;
  parent_code?: string;
}

export interface RoleFuncItem {
  func_code: string;
}

// export const role_status = {
//   [-999]: '全部',
//   [-100]: {
//     text: '作废',
//     status: 'Error',
//   },
//   [0]: {
//     text: '正常',
//     status: 'Success',
//   },
// };
