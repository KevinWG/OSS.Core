const FuncCodes = {
  //====== 用户管理
  Portal: 'portal',

  Portal_UserAdd: 'portal_user_add',
  Portal_UserList: 'portal_user_list',
  Portal_UserLock: 'portal_user_lock',
  Portal_UserUnLock: 'portal_user_unlock',

  Portal_AdminList: 'portal_admin_list',
  Portal_AdminCreate: 'portal_admin_create',
  Portal_AdminLock: 'portal_admin_lock',
  Portal_AdminUnLock: 'portal_admin_unlock',
  Portal_AdminSetType: 'portal_admin_settype',

  //====== 权限管理
  Permit: 'permit',

  Permit_RoleList: 'permit_role_list',
  Permit_RoleAdd: 'permit_role_add',
  Permit_RoleUpdate: 'permit_role_update',
  Permit_RoleActive: 'permit_role_active',
  Permit_RoleDelete: 'permit_role_delete',

  Permit_RoleFuncList: 'permit_rolefunc_list',
  Permit_RoleFuncChange: 'permit_rolefunc_change',

  Permit_RoleUserSearch: 'permit_roleuser_search',
  Permit_RoleUserBind: 'permit_roleuser_bind',
  Permit_RoleUserDelete: 'permit_roleuser_delete',
};

export default FuncCodes;
