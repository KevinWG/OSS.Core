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
        path: '/portal/permit',
        access: FuncCodes.Poral_Permit,
        name: '权限管理',
        routes: [
          {
            path: '/portal/permit/role',
            name: '角色管理',
            access: FuncCodes.portal_role_list,
            component: './portal/permit/role/list',
          },
          {
            path: '/portal/permit/func',
            name: '权限项管理',
            access: FuncCodes.Portal_Func_Operate,
            component: './portal/permit/func_items',
          },
          { component: './404' },
        ],
      },
      {
        name: '登录设置',
        access: FuncCodes.portal_setting_auth,
        path: '/portal/auth/setting',
        component: './portal/auth_setting',
      },
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
