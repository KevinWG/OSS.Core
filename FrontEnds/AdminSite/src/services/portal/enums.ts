export enum PortalAuthorizeType {
  // 超级管理员 - 管理端
  SuperAdmin = 100,
  // 后台普通管理员- 管理端
  Admin = 200,
  // 用户
  User = 300,
  // 空白用户（关键信息缺失，如手机号）
  UserWithEmpty = 310,
  // 临时授权的第三方用户
  SocialAppUser = 400,
}

export enum FuncDataLevel {
  //  全部
  All = 1,
  //  组织树
  GroupTree = 20,
  // 当前组织
  Group = 40,
  //  仅个人
  OnlySelf = 60,
}

export enum PortalNameType {
  // 手机号
  Mobile = 1,
  // 邮箱
  Email = 2,
}

export enum PortalType {
  /**系统账号密码 */
  Password = 0,

  /**动态验证码 */
  Code = 1,
}

export const UserStatus = {
  '-100': {
    text: '锁定',
    status: 'Error',
  },
  '-20': {
    text: '待绑定',
    status: 'Warning',
  },
  '0': {
    text: '正常',
    status: 'Success',
  },
};

export const AdminType = {
  '0': {
    text: '普通',
  },
  '100': {
    text: '超级管理员',
  },
};
