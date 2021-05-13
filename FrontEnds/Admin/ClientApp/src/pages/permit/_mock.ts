import { Request, Response } from 'express';
import { createTestPageList, success } from '../../mock/_mock';
import { RoleInfo, FuncItem, RoleFuncItem, RoleUserInfo } from './data_d';
import { ListResp } from '@/utils/resp_d';

function searchLists(req: Request, res: Response, u: string) {
  const pageRes = createTestPageList<RoleInfo>(200, (i: number) => ({
    id: i.toString(),
    name: `roleName ${i}`,
    status: i % 2 == 1 ? -100 : 0,
    add_time: 1589553024,
  }));
  return res.send(pageRes);
}

function getAllFuncItems(req: Request, res: Response, u: string) {
  const list: FuncItem[] = [
    { title: '添加角色', code: 'role_add', parent_code: 'role_list' },
    { title: '角色管理', code: 'role_list' },
    { title: '用户列表', code: 'user_list' },
    { title: '测试管理', code: 'test_manager' },
    { title: '测试列表', code: 'test_list', parent_code: 'test_manager' },
    { title: '详情修改', code: 'detail_Chang', parent_code: 'test_manager' },
    { title: '独立列表', code: 'mmmmmm_list' },
  ];

  const listRes: ListResp<FuncItem> = {
    ret: 0,
    msg: '',
    data: list,
  };
  return res.send(listRes);
}

function getRoleFuncItems(req: Request, res: Response, u: string) {
  const list: RoleFuncItem[] = [
    { func_code: 'role_add' },
    { func_code: 'role_list' },
    { func_code: 'test_list' },
  ];

  const listRes: ListResp<RoleFuncItem> = {
    ret: 0,
    msg: '',
    data: list,
  };
  return res.send(listRes);
}

function searchUserLists(req: Request, res: Response, u: string) {
  const pageRes = createTestPageList<RoleUserInfo>(200, (i: number) => ({
    id: i.toString(),
    u_id: i.toString(),
    r_id: i.toString(),
    name: ``,
    r_name: `测试角色 ${i}`,
    u_name: `管理员 ${i}`,
    status: i % 2 == 1 ? -100 : 0,
    add_time: 1589553024,
  }));
  pageRes.data = pageRes.data.map((u, index) => {
    const i = Math.ceil(index / 3);
    u.name = '角色 ' + i;
    return u;
  });
  return res.send(pageRes);
}

export default {
  'POST /api/b/permit/SearchRoles': searchLists,
  'POST /api/b/permit/RoleActive': success,
  'POST /api/b/permit/roleUnActive': success,
  'POST /api/b/permit/RoleDelete': success,
  'POST /api/b/permit/roleadd': success,
  'POST /api/b/permit/RoleUpdate': success,

  'GET /api/b/permit/GetAllFuncItems': getAllFuncItems,
  'GET /api/b/permit/GetRoleFuncList': getRoleFuncItems,
  'POST /api/b/permit/ChangeRoleFuncItems': success,

  'POST /api/b/permit/SearchRoleUsers': searchUserLists,
  'POST /api/b/permit/DeleteRoleBind': success,
  'POST /api/b/permit/AddRoleBind': success,
};
