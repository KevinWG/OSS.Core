import { Request, Response } from 'express';

export const createTestPageList = function createTestPageList<T>(count: number, itemGenerator: (index: number) => T) {
    const testPageList: T[] = [];
    for (let i = 0; i < count; i++) {
        testPageList.push(itemGenerator(i));
    }
    return {
        data: testPageList,
        total: count, //dataSource.length,
        is_ok: true,
        is_failed: true,
        ret: 0,
        msg: ''
    }
}

export function success(req: Request, res: Response, u: string) {
    res.send({
        ret: 0,
        msg: '成功',
    });
}


export default {
    'Get /api/home/success': success,
  };
  