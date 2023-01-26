using OSS.Common.Resp;
using OSS.Core.Client.Http;
using OSS.Core.Context;

namespace OSS.Core.Module.Portal.Client;

/// <inheritdoc />
internal class AuthHttpClient : IAuthOpenService
{
    /// <inheritdoc />
    public Task<IResp<UserIdentity>> GetIdentity()
    {
        return new PortalRemoteReq("/Auth/GetIdentity")
            .GetAsync<IResp<UserIdentity>>();
    }

    /// <inheritdoc />
    public Task<IResp> CheckIfCanReg(PortalNameReq req)
    {
        return new PortalRemoteReq("/Auth/CheckIfCanReg")
            .PostAsync<IResp>(req);
    }

    /// <inheritdoc />
    public Task<PortalTokenResp> CodeLogin(PortalPasscodeReq req)
    {
        return new PortalRemoteReq("/Auth/CodeLogin")
            .PostAsync<PortalTokenResp>(req);
    }

    /// <inheritdoc />
    public Task<PortalTokenResp> CodeAdminLogin(PortalPasscodeReq req)
    {
        return new PortalRemoteReq("/Auth/CodeAdminLogin")
            .PostAsync<PortalTokenResp>(req);
    }

    /// <inheritdoc />
    public Task<PortalTokenResp> CodeRegOrLogin(PortalPasscodeReq req)
    {
        return new PortalRemoteReq("/Auth/CodeRegOrLogin")
            .PostAsync<PortalTokenResp>(req);
    }


    /// <inheritdoc />
    public Task<IResp> SendCode(PortalNameReq req)
    {
        return new PortalRemoteReq("/Auth/SendCode")
            .PostAsync<IResp>(req);
    }

    /// <inheritdoc />
    public Task<PortalTokenResp> PwdReg(PortalPasswordReq req)
    {
        return new PortalRemoteReq("/Auth/PwdReg")
            .PostAsync<PortalTokenResp>(req);
    }

    /// <inheritdoc />
    public Task<PortalTokenResp> PwdLogin(PortalPasswordReq req)
    {
        return new PortalRemoteReq("/Auth/PwdLogin")
            .PostAsync<PortalTokenResp>(req);
    }

    /// <inheritdoc />
    public Task<PortalTokenResp> PwdAdminLogin(PortalPasswordReq req)
    {
        return new PortalRemoteReq("/Auth/PwdAdminLogin")
            .PostAsync<PortalTokenResp>(req);
    }
}