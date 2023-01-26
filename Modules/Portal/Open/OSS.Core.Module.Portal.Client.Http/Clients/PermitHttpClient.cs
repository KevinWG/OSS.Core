using OSS.Common.Resp;
using OSS.Core.Client.Http;
using OSS.Core.Context;

namespace OSS.Core.Module.Portal.Client;

internal class PermitHttpClient : IGrantOpenService
{
    /// <inheritdoc />
    public Task<ListResp<GrantedPermit>> GetCurrentUserPermits()
    {
        return new PortalRemoteReq("/Permit/GetCurrentUserPermits")
            .GetAsync<ListResp<GrantedPermit>>();
    }

    /// <inheritdoc />
    public Task<IResp<FuncDataLevel>> CheckPermit(string funcCode)
    {
        return new PortalRemoteReq($"/Permit/CheckPermit?func_code={funcCode}")
            .PostAsync<IResp<FuncDataLevel>>();
    }

    /// <inheritdoc />
    public Task<ListResp<GrantedPermit>> GetPermitsByRoleId(long roleId)
    {
        return new PortalRemoteReq($"/Permit/GetPermitsByRoleId?roleId={roleId}")
            .GetAsync<ListResp<GrantedPermit>>();
    }

    /// <inheritdoc />
    public Task<IResp> ChangeRolePermits(long rid, ChangeRolePermitReq req)
    {
        return new PortalRemoteReq($"/Permit/ChangeRolePermits?rid={rid}")
            .PostAsync<IResp>(req);
    }
}

