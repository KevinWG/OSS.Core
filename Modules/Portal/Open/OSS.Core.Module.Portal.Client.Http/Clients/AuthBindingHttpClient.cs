using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client;

internal class AuthBindingHttpClient : IAuthBindingOpenService
{
    public Task<Resp<UserBasicMo>> GetCurrentUser()
    {
        return new PortalRemoteRequest("/AuthBinding/GetCurrentUser")
            .GetAsync<Resp<UserBasicMo>>();
    }

    public Task<Resp<long>> AddUser(AddUserReq req)
    {
        return new PortalRemoteRequest("/AuthBinding/AddUser")
            .PostAsync<Resp<long>>(req);
    }

    public Task<IResp> SendOldCode(PortalNameType type)
    {
        return new PortalRemoteRequest("/AuthBinding/SendOldCode?type=" + (int) type)
            .PostAsync<IResp>();
    }

    public Task<IResp> SendNewCode(PortalNameReq req)
    {
        return new PortalRemoteRequest("/AuthBinding/SendNewCode")
            .PostAsync<IResp>(req);
    }

    public Task<GetBindTokenResp> GetBindToken(string oldCode, PortalNameType type)
    {
        return new PortalRemoteRequest(string.Concat("/AuthBinding/GetBindToken?oldCode=", oldCode, "&type=", type))
            .GetAsync<GetBindTokenResp>();
    }

    public Task<IResp> Bind(BindByPassCodeReq req)
    {
        return new PortalRemoteRequest("/AuthBinding/Bind")
            .PostAsync<IResp>(req);
    }
}