using OSS.Common.Resp;
using OSS.Core.Client.Http;
using OSS.Core.Context;

namespace OSS.Core.Module.Portal.Client;

internal class GrantHttpClient : IGrantOpenService
{
    public Task<ListResp<GrantedPermit>> GetCurrentUserPermits()
    {
        return new PortalRemoteReq("/Func/GetCurrentUserPermits")
            .GetAsync<ListResp<GrantedPermit>>();
    }

    public Task<IResp<FuncDataLevel>> CheckPermit(string funcCode)
    {
        return new PortalRemoteReq("/Func/CheckPermit?func_code=" + funcCode)
            .PostAsync<IResp<FuncDataLevel>>();
    }

    public Task<ListResp<GrantedPermit>> GetPermitsByRoleId(long roleId)
    {
        return new PortalRemoteReq("/Func/GetPermitsByRoleId?rid=" + roleId)
            .GetAsync<ListResp<GrantedPermit>>();
    }

    public Task<IResp> ChangeRolePermits(long rid, ChangeRolePermitReq req)
    {
        return new PortalRemoteReq("/Func/ChangeRolePermits?rid=" + rid)
            .PostAsync<IResp>(req);
    }
}