using OSS.Common.Resp;
using OSS.Core.Client.Http;
using OSS.Core.Context;

namespace OSS.Core.Module.Portal.Client;

/// <inheritdoc />
internal class AuthClient : IOpenedAuthService
{
    /// <inheritdoc />
    public Task<IResp<UserIdentity>> GetIdentity()
    {
        return new PortalRemoteRequest("/Auth/GetIdentity")
            .GetAsync<IResp<UserIdentity>>();
    }

    /// <inheritdoc />
    public Task<IResp> CheckIfCanReg(PortalNameReq req)
    {
        return new PortalRemoteRequest("/Auth/CheckIfCanReg")
            .PostAsync<IResp>(req);
    }

    /// <inheritdoc />
    public Task<PortalTokenResp> CodeLogin(PortalPasscodeReq req)
    {
        return new PortalRemoteRequest("/Auth/CodeLogin")
            .PostAsync<PortalTokenResp>(req);
    }

    /// <inheritdoc />
    public Task<PortalTokenResp> CodeAdminLogin(PortalPasscodeReq req)
    {
        return new PortalRemoteRequest("/Auth/CodeAdminLogin")
            .PostAsync<PortalTokenResp>(req);
    }

    /// <inheritdoc />
    public Task<PortalTokenResp> CodeRegOrLogin(PortalPasscodeReq req)
    {
        return new PortalRemoteRequest("/Auth/CodeRegOrLogin")
            .PostAsync<PortalTokenResp>(req);
    }


    /// <inheritdoc />
    public Task<IResp> SendCode(PortalNameReq req)
    {
        return new PortalRemoteRequest("/Auth/SendCode")
            .PostAsync<IResp>(req);
    }

    /// <inheritdoc />
    public Task<PortalTokenResp> PwdReg(PortalPasswordReq req)
    {
        return new PortalRemoteRequest("/Auth/PwdReg")
            .PostAsync<PortalTokenResp>(req);
    }

    /// <inheritdoc />
    public Task<PortalTokenResp> PwdLogin(PortalPasswordReq req)
    {
        return new PortalRemoteRequest("/Auth/PwdLogin")
            .PostAsync<PortalTokenResp>(req);
    }

    /// <inheritdoc />
    public Task<PortalTokenResp> PwdAdminLogin(PortalPasswordReq req)
    {
        return new PortalRemoteRequest("/Auth/PwdAdminLogin")
            .PostAsync<PortalTokenResp>(req);
    }
}