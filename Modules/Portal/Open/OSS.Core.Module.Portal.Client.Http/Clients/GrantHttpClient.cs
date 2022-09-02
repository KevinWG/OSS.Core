using OSS.Common.Resp;
using OSS.Core.Client.Http;
using OSS.Core.Context;

namespace OSS.Core.Module.Portal.Client;

internal class GrantHttpClient : IGrantOpenService
{
    public Task<ListResp<GrantedPermit>> GetCurrentUserPermits()
    {
        return new PortalRemoteRequest("/Func/GetCurrentUserPermits")
            .GetAsync<ListResp<GrantedPermit>>();
    }

    public Task<IResp<FuncDataLevel>> CheckPermit(string funcCode)
    {
        return new PortalRemoteRequest("/Func/CheckPermit?func_code=" + funcCode)
            .PostAsync<IResp<FuncDataLevel>>();
    }

    public Task<ListResp<GrantedPermit>> GetPermitsByRoleId(long roleId)
    {
        return new PortalRemoteRequest("/Func/GetPermitsByRoleId?rid=" + roleId)
            .GetAsync<ListResp<GrantedPermit>>();
    }

    public Task<IResp> ChangeRolePermits(long rid, ChangeRolePermitReq req)
    {
        return new PortalRemoteRequest("/Func/ChangeRolePermits?rid=" + rid)
            .PostAsync<IResp>(req);
    }
}