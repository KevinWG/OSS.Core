import FuncCodes from './services/common/func_codes';

/**
 * @see https://umijs.org/zh-CN/plugins/plugin-access
 * */
export default function access(initialState: { currentUser?: PortalApi.UserIdentity } | undefined) {
  const { currentUser } = initialState ?? {};
  const { access_list } = currentUser ?? {};

  const access: any = {};

  for (var code in FuncCodes) {
    access[FuncCodes[code]] = false;
  }

  if (!access_list) {
    return access;
  }

  access_list.forEach((a) => {
    access[a.func_code] = true;
  });

  return access;
}
