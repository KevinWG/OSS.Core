using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;

namespace OSS.Core.Module.Portal;

/// <summary>
///  登录用户授权验证
/// </summary>
public class UserAuthProvider : IUserAuthProvider
{
    private static readonly AuthService _auth = new();
    /// <inheritdoc />
    public Task<IResp<UserIdentity>> GetIdentity()
    {
        return _auth.GetIdentity();
    }
}

/// <summary>
///  登录用户功能授权验证
/// </summary>
public class FuncAuthProvider : IFuncAuthProvider
{
    private static readonly PermitService _permit = new();
    /// <inheritdoc />
    public Task<IResp<FuncDataLevel>> Authorize(string funcCode, string sceneCode)
    {
        return _permit.CheckPermit(funcCode, sceneCode);
    }
}