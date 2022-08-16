using OSS.Common.Resp;
using OSS.Core.Client.Http;
using OSS.Core.Context;

namespace OSS.Core.Module.Portal.Client.Http;

/// <inheritdoc />
internal class AuthClient : IOpenedAuthService
{
    /// <inheritdoc />
    public Task<IResp<UserIdentity>> GetIdentity()
    {
        return new PortalRequest("/Auth/GetIdentity")
            .GetAsync<IResp<UserIdentity>>();
    }
    /// <inheritdoc />
    public Task<IResp> CheckIfCanReg(PortalNameReq req)
    {
        return new PortalRequest("/Auth/CheckIfCanReg")
            .PostAsync<IResp>(req);
    }
    /// <inheritdoc />
    public Task<PortalTokenResp> CodeLogin(PortalPasscodeReq req)
    {
        return new PortalRequest("/Auth/CodeLogin")
            .PostAsync<PortalTokenResp>(req);
    }
    /// <inheritdoc />
    public Task<PortalTokenResp> CodeAdminLogin(PortalPasscodeReq req)
    {
        return new PortalRequest("/Auth/CodeAdminLogin")
            .PostAsync<PortalTokenResp>(req);
    }
    /// <inheritdoc />
    public Task<PortalTokenResp> CodeRegOrLogin(PortalPasscodeReq req)
    {
        return new PortalRequest("/Auth/CodeRegOrLogin")
            .PostAsync<PortalTokenResp>(req);
    }
    /// <inheritdoc />
    public Task<IResp> SendCode(PortalNameReq req)
    {
        return new PortalRequest("/Auth/SendCode")
            .PostAsync<IResp>(req);
    }

    /// <inheritdoc />
    public Task<PortalTokenResp> PwdReg(PortalPasswordReq req)
    {
        return new PortalRequest("/Auth/PwdReg")
            .PostAsync<PortalTokenResp>(req);
    }

    /// <inheritdoc />
    public Task<PortalTokenResp> PwdLogin(PortalPasswordReq req)
    {
        return new PortalRequest("/Auth/PwdLogin")
            .PostAsync<PortalTokenResp>(req);
    }

    /// <inheritdoc />
    public Task<PortalTokenResp> PwdAdminLogin(PortalPasswordReq req)
    {
        return new PortalRequest("/Auth/PwdAdminLogin")
            .PostAsync<PortalTokenResp>(req);
    }
}