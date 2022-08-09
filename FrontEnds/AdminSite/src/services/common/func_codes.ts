const FuncCodes = {
  portal: 'portal',
  portal_setting_auth: 'portal_setting_auth',

  portal_user_add: 'portal_user_add',
  portal_user_list: 'portal_user_list',

  portal_user_lock: 'portal_user_lock',
  portal_user_unlock: 'portal_user_unlock',

  //========  角色管理

  portal_user_roles: 'portal_user_roles',
  portal_role_list: 'portal_role_list',
  portal_role_add: 'portal_role_add',
  portal_role_active: 'portal_role_active',

  portal_role_bind_user: 'portal_role_bind_user',
  portal_role_bind_delete: 'portal_role_bind_delete',

  Poral_Permit: 'portal_permit',
  Portal_Func_Operate: 'portal_func_operate', // 权限项修改编辑操作

  portal_grant_role_permits: 'portal_grant_role_permits',
  portal_grant_role_change: 'portal_grant_role_change',

  // =====  管理员权限
  portal_admin_list: 'portal_admin_list',
  portal_admin_create: 'portal_admin_create',
  portal_admin_lock: 'portal_admin_lock',
  portal_admin_unlock: 'portal_admin_unlock',
  portal_admin_settype: 'portal_admin_settype',

  // ======= 通知中心
  Notify: 'notify',
  Notify_Template_List: 'notify_template_list',
  Notify_Template_Add: 'notify_template_add',
  Notify_Template_Update: 'notify_template_update',
};

export default FuncCodes;
