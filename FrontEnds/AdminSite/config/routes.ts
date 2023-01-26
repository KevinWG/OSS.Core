import FuncCodes from '../src/services/common/func_codes';

export default [
  {
    path: '/user',
    layout: false,
    routes: [
      { name: 'login', path: '/user/login', component: './portal/login' },
      { component: './404' },
    ],
  },
  {
    path: '/portal',
    access: FuncCodes.portal,
    routes: [
      {
        name: '用户列表',
        access: FuncCodes.portal_user_list,
        path: '/portal/user/list',
        component: './portal/user/list',
      },
      {
        name: '管理员列表',
        access: FuncCodes.portal_admin_list,
        path: '/portal/admin/list',
        component: './portal/admin/list',
      },
      {
        name: '合作对象管理',
        access: FuncCodes.portal_org,
        path: '/portal/org/list',
        component: './portal/organization/list',
      },

      {
        name: '登录设置',
        access: FuncCodes.portal_setting_auth,
        path: '/portal/auth/setting',
        component: './portal/auth_setting',
      },
    ],
  },
  {
    path: '/permit',
    access: FuncCodes.Poral_Permit,
    name: '权限管理',
    routes: [
      {
        path: '/permit/role',
        name: '角色管理',
        access: FuncCodes.portal_role_list,
        component: './portal/permit/role/list',
      },
      {
        path: '/permit/func',
        name: '权限项管理',
        access: FuncCodes.Portal_Func_Operate,
        component: './portal/permit/func_items',
      },
      { component: './404' },
    ],
  },
  // {
  //   path: '/permit',
  //   access: FuncCodes.Permit,
  //   routes: [
  //     { path: '/permit/func', name: '权限码列表', component: './permit/func_items' },
  //     { component: './404' },
  //   ],
  // },
  {
    path: '/product',
    name: '商品管理',
    access: FuncCodes.Product,
    routes: [
      {
        path: '/product/category/list',
        access: FuncCodes.Product_SpuCategory,
        name: '商品分类',
        component: './product/category/list',
      },
      { component: './404' },
    ],
  },
  {
    path: '/wms',
    name: '仓储管理',
    access: FuncCodes.WMS,
    routes: [
      {
        path: '/wms/stock/apply',
        name: '出入库申请',
        access: FuncCodes.wms_stock_apply_list,
        component: './wms/stock_apply/list',
      },
      {
        path: '/wms/mcategory/list',
        access: FuncCodes.WMS_MCategory,
        name: '物料分类',
        component: './wms/mcategory/list',
      },
      {
        path: '/wms/material/list',
        name: '物料管理',
        access: FuncCodes.wms_material,
        component: './wms/material/list',
      },
      {
        path: '/wms/warehouse/list',
        access: FuncCodes.WMS_Warehouse,
        name: '仓库管理',
        component: './wms/warehouse/list',
      },
      {
        path: '/wms/batch/list',
        name: '批次管理',
        access: FuncCodes.wms_batch_msearch,
        component: './wms/batch/list',
      },
      {
        path: '/wms/unit/list',
        access: FuncCodes.wms_unit_list,
        name: '单位设置',
        component: './wms/unit/list',
      },
      { component: './404' },
    ],
  },
  {
    path: '/notify',
    access: FuncCodes.Notify,
    routes: [
      {
        path: '/notify/template/list',
        name: '通知模板',
        access: FuncCodes.Notify_Template_List,
        component: './notify/template',
      },
      { component: './404' },
    ],
  },

  { path: '/welcome', hideInMenu: true, name: 'welcome', component: './Welcome' },
  { path: '/', redirect: '/welcome' },

  { path: '/404', layout: false, component: './404' },
  { component: './404' },
];
