using OSS.Common;

using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Attributes;
using OSS.Core.Module.Portal;

namespace OSS.Core;

/// <inheritdoc />
public class FuncAuthDefaultProvider : IFuncAuthProvider
{
    /// <inheritdoc />
    public async Task<IResp> Authorize(AskUserFunc askFunc)
    {
        var authType  = askFunc?.auth_type ?? PortalAuthorizeType.User;
        var funcCode  = askFunc?.func_code;
        var sceneCode = askFunc?.scene_code;

        var userIdentity = CoreContext.User.Identity;
        if (userIdentity.auth_type == PortalAuthorizeType.SuperAdmin)
        {
            userIdentity.data_level = FuncDataLevel.All;
            return new Resp();
        }

        if (userIdentity.auth_type > authType)
        {
            switch (userIdentity.auth_type)
            {
                case PortalAuthorizeType.SocialAppUser:
                    return new Resp(RespCodes.UserFromSocial, "需要绑定系统账号");
                case PortalAuthorizeType.UserWithEmpty:
                    return new Resp(RespCodes.UserIncomplete, "需要绑定手机号!");
            }

            return new Resp().WithResp(RespCodes.UserNoPermission, "权限不足!");
        }

        var checkRes = await CheckIfHaveFunc(funcCode, sceneCode);
        if (checkRes.IsSuccess())
        {
            userIdentity.data_level = checkRes.data;
        }

        return checkRes;
    }

    /// <summary>
    ///  判断登录用户是否具有某权限
    /// </summary>
    /// <param name="funcCode"></param>
    /// <param name="sceneCode"></param>
    /// <returns></returns>
    public async Task<IResp<FuncDataLevel>> CheckIfHaveFunc(string funcCode, string sceneCode)
    {
        if (string.IsNullOrEmpty(funcCode))
        {
            return new Resp<FuncDataLevel>(FuncDataLevel.All);
        }

        var userFunc = await InsContainer<IPortalClient>.Instance.Permit.GetCurrentUserPermits();
        if (!userFunc.IsSuccess())
            return new Resp<FuncDataLevel>().WithResp(userFunc);

        var fullFuncCode = string.IsNullOrEmpty(sceneCode) ? funcCode : string.Concat(funcCode, ":", sceneCode);
        var func         = userFunc.data.FirstOrDefault(f => f.func_code == fullFuncCode);

        if (func == null)
            return new Resp<FuncDataLevel>().WithResp(RespCodes.UserNoPermission, "无操作权限!");

        return new Resp<FuncDataLevel>(func.data_level);
    }

}