export enum RespCodes {
  /**成功 */
  Success = 0,
  /**参数错误 */
  ParaError = 1300, // 0x00000514
  /**  参数过期  */
  ParaExpired = 1310, // 0x0000051E
  /**签名错误 */
  ParaSignError = 1312, // 0x00000520
  /**未登录 */
  UserUnLogin = 1401, // 0x00000579
  /**已拉黑 */
  UserBlocked = 1403, // 0x0000057B
  /**权限不足 */
  UserNoPermission = 1405, // 0x0000057D
  /**账号未激活 */
  UserUnActive = 1424, // 0x00000590
  /**第三方授权待绑定 */
  UserFromSocial = 1426, // 0x00000592
  /**账号信息缺失 */
  UserIncomplete = 1428, // 0x00000594
  /**操作失败 */
  OperateFailed = 1500, // 0x000005DC
  /**对象不存在 */
  OperateObjectNull = 1504, // 0x000005E0
  /**对象已存在 */
  OperateObjectExist = 1506, // 0x000005E2
}

export const CommonStatus = {
  '-100': {
    text: '锁定',
    status: 'Error',
  },
  '0': {
    text: '正常',
    status: 'Success',
  },
};
