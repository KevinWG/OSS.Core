import FuncCodes from './func_codes';

const PortalConfig = {
  login_url: '/login',
};

const routes = [
  {
    path: '/',
    name: '欢迎回来',
    icon: 'home',
    hideInMenu: true,
    component: './home/Welcome',
  },
  {
    path: '/login',
    layout: false,
    hideInMenu: true,
    component: './portal/login',
  },
  {
    name: '我的信息',
    icon: 'user',
    hideInMenu: true,
    path: '/portal/admin/myprofile',
    component: './portal/admin/my_profile',
  },
  {
    name: '用户管理',
    icon: 'user',
    path: '/portal',
    access: FuncCodes.Portal_UserList,
    routes: [
      {
        name: '用户列表',
        icon: 'user',
        path: '/portal/user',
        access: FuncCodes.Portal_UserList,
        component: './portal/user/list',
      },
      {
        name: '管理员列表',
        icon: 'user',
        path: '/portal/admin/list',
        access: FuncCodes.Portal_AdminList,
        component: './portal/admin/list',
      },
    ],
  },
  {
    name: '权限管理',
    icon: 'Flag',
    path: '/permit',
    access: FuncCodes.Permit,
    routes: [
      {
        name: '角色列表',
        icon: 'DeploymentUnit',
        path: '/permit/role/list',
        access: FuncCodes.Permit_RoleList,
        component: './permit/role_list',
      },
      {
        name: '用户角色绑定',
        icon: 'LinkOutlined',
        path: '/permit/role/users',
        access: FuncCodes.Permit_RoleUserSearch,
        component: './permit/role_user_list',
      },
    ],
  },
  
  {
    component: './404',
  },
];

export default routes;

export { PortalConfig };
