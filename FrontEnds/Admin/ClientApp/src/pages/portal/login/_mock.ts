import { Request, Response } from 'express';
import { FuncCodes } from '../../../utils/resp_d';
import { success } from '../../../mock/_mock';

let funcCodeList: [] = [];

for (var code in FuncCodes) {
  funcCodeList.push({ func_code: FuncCodes[code], data_level: 1 });
}

function getFakeCaptcha(req: Request, res: Response) {
  return res.json('captcha-xxx');
}

function adminLogin(req: Request, res: Response) {
  //const { password, userName, type } = req.body;
  res.send({
    ret: 0,
    msg: '',
    data: {
      id: '1',
      name: '登录账号',
      avatar: 'https://gw.alipayobjects.com/zos/antfincdn/XAosXuNZyF/BiazfanxmamNRoxxVxka.png',
    },
  });
}
// 代码中会兼容本地 service mock 以及部署站点的静态数据
export default {
  'GET /api/b/portal/GetMyself': (req: Request, res: Response) => {
    res.send({
      ret: 0,
      msg: '',
      data: {
        id: '1',
        name: '首页账号',
        avatar: 'https://gw.alipayobjects.com/zos/antfincdn/XAosXuNZyF/BiazfanxmamNRoxxVxka.png',
      },
    });
  },
  'GET /api/b/permit/GetMyFuncs': (req: Request, res: Response) => {
    res.send({
      ret: 0,
      msg: '',
      data: funcCodeList,
    });
  },

  'GET /api/b/portal/Logout': (req: Request, res: Response) => {
    res.send({ data: {}, success: true });
  },

  'POST /api/b/portal/codeadminlogin': adminLogin,
  'POST /api/b/portal/PwdAdminLogin': adminLogin,
  'GET  /api/login/captcha': getFakeCaptcha,
  'POST /api/portal/SendCode': success,
};
