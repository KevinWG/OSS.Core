const FuncCodes = {
  portal: 'portal',
  portal_setting_auth: 'portal_setting_auth',

  portal_user_add: 'portal_user_add',
  portal_user_list: 'portal_user_list',

  portal_user_lock: 'portal_user_lock',
  portal_user_unlock: 'portal_user_unlock',

  portal_org: 'portal_org',
  portal_org_manage: 'portal_org_manage',

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

  // ======= 商品管理
  Product: 'product',
  Product_SpuCategory: 'product_spucategory',
  Product_SpuCategory_Manage: 'product_spucategory_manage',

  // ======= 仓储管理
  WMS: 'wms',
  WMS_MCategory: 'wms_mcategory',
  WMS_MCategory_Manage: 'wms_mcategory_manage',

  WMS_Warehouse: 'wms_warehouse',
  WMS_Warehouse_Manage: 'wms_warehouse_manage',

  WMS_WareArea: 'wms_warearea',
  WMS_WareArea_Manage: 'wms_warearea_manage',

  wms_material: 'wms_material',
  wms_material_manage: 'wms_material_manage',

  wms_stock_apply_list: 'wms_stock_apply_list',
  wms_stock_apply_manage: 'wms_stock_apply_manage',

  wms_unit_list: 'wms_unit_list',
  wms_unit_manage: 'wms_unit_manage',

  wms_batch_msearch: 'wms_batch_msearch',
  wms_batch_modify: 'wms_batch_modify',
};

export default FuncCodes;
