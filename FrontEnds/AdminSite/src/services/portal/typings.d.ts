declare namespace PortalApi {
  interface UserIdentity {
    // 授权类型
    auth_type: PortalAuthorizeType;

    // 授权编号
    id: string;

    // 展示名称
    name: string;

    // 展示图片
    avatar: string;

    access_list: AccessFunItem[];
  }
  type AccessFunItem = {
    func_code: string;
    data_level: number;
  };

  interface IPortalNameReq {
    type: PortalNameType;
    name: string;
  }

  interface IPortalReq extends IPortalNameReq {
    is_social_bind: number;
  }

  interface AuthResp extends IRespData<UserIdentity> {
    token: string;
  }

  /**  登录请求响应 ====== start ====== */

  /** 密码登录请求 */
  type PasswordAuthReq = {
    password: string;
  } & IPortalReq;

  /**  动态码登录请求 */
  type CodeAuthReq = {
    code: string;
  } & IPortalReq;

  interface AuthSetting {
    AuthSmsTemplateId: string;
    AuthEmailTemplateId: string;
    BindSmsTemplateId: string;
    BindEmailTemplateId: string;
  }

  interface UserInfo extends BaseMo {
    nick_name: string;
    avatar: string;
    email: string;
    mobile: string;
  }

  /**  管理员信息  */
  interface AdminInfo extends BaseMo {
    admin_name: string;
    avatar: string;
    admin_type: number;
  }

  interface CreateAdminReq {
    id: string;
    admin_name: string;
  }

  interface RoleMo extends BaseMo {
    name: string;
  }

  export interface RoleFuncItem {
    func_code: string;
  }

  /**
   * 用户角色绑定实体
   */
  interface RoleUserBind extends BaseMo {
    name: string;
    u_id: string;
    role_id: string;
  }
}
