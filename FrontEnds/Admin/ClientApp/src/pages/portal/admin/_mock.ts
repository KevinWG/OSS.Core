import { AdminInfo } from './data_d';
import { Request, Response } from 'express';

let tableListDataSource: AdminInfo[] = [];

for (let i = 0; i < 8; i += 1) {
  const id = i.toString();

  let user = {
    id: id,
    u_id: id,
    avatar: [
      'https://gw.alipayobjects.com/zos/rmsportal/eeHMaZBwmTvLdIwMfBpg.png',
      'https://gw.alipayobjects.com/zos/rmsportal/udxAbMEhpwthVVcjLXik.png',
    ][i % 2],
    admin_name: `TradeCode ${i}`,
    email: '1@qq.com',
    status: i % 2 == 1 ? -20 : 0,
    admin_type: i % 2 == 1 ? 100 : 0,
    add_time: 1589553024,
  };

  if (i < 2) {
    user.status = -100;
  }
  tableListDataSource.push(user);
}

function searchAdmins(req: Request, res: Response, u: string) {
  const result = {
    data: tableListDataSource,
    total: 200, //dataSource.length,
    success: true,
  };

  return res.send(result);
}
function success(req: Request, res: Response, u: string) {
  res.send({
    ret: 0,
    msg: '成功',
  });
}
export default {
  'POST /api/b/admin/searchadmins': searchAdmins,
  'POST /api/b/admin/create': success,
  'POST /api/b/admin/lock': success,
  'POST /api/b/admin/unlock': success,
  'POST /api/b/admin/SetAdminType': success,
};
