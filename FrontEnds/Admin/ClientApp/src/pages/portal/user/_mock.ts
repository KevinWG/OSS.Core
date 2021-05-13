// eslint-disable-next-line import/no-extraneous-dependencies
import { Request, Response } from 'express';
import { UserInfo } from './data_d';
import { success } from '../../../mock/_mock';

let tableListDataSource: UserInfo[] = [];

for (let i = 0; i < 8; i += 1) {
  const id = i.toString();

  let user = {
    id: id,
    avatar: [
      'https://gw.alipayobjects.com/zos/rmsportal/eeHMaZBwmTvLdIwMfBpg.png',
      'https://gw.alipayobjects.com/zos/rmsportal/udxAbMEhpwthVVcjLXik.png',
    ][i % 2],
    nick_name: `TradeCode ${i}`,
    email: '1@qq.com',
    status: i % 2 == 1 ? -20 : 0,
    add_time: 1589553024,
  };

  if (i < 2) {
    user.status = -100;
  }
  tableListDataSource.push(user);
}

function getUsers(req: Request, res: Response, u: string) {
  const result = {
    data: tableListDataSource,
    total: 200, //dataSource.length,
    success: true,
  };

  return res.send(result);
}

function lockUser(req: Request, res: Response, u: string) {
  res.send({ ret: 0 });
}
export default {
  'POST /api/b/portal/CheckIfCanReg': success,

  'POST /api/b/user/AddUser': success,
  'POST /api/b/user/searchusers': getUsers,
  'POST /api/b/user/lock': lockUser,
  'POST /api/b/user/unlock': lockUser,
};
