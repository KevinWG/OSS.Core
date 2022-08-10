using OSS.Common;

using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Module.Portal;

namespace OSS.Core;

/// <inheritdoc />
public class DefaultFuncAuthProvider : IFuncAuthProvider
{
    /// <inheritdoc />
   public  async Task<IResp<FuncDataLevel>> Authorize(AskUserFunc askFunc)
    {
        var funcCode  = askFunc?.func_code;
        var sceneCode = askFunc?.scene_code;

        return await CheckIfHaveFunc(funcCode, sceneCode);
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
            return new Resp<FuncDataLevel>(FuncDataLevel.All);

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