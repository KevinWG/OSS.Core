using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client.Http;

internal class PermitClient : IOpenedPermitService
{
    public Task<ListResp<GrantedPermit>> GetCurrentUserPermits()
    {
        return new PortalRequest("/Permit/GetCurrentUserPermits")
            .GetAsync<ListResp<GrantedPermit>>();
    }

    public Task<ListResp<GrantedPermit>> GetPermitsByRoleId(long roleId)
    {
        return new PortalRequest($"/Permit/GetPermitsByRoleId?roleId={roleId}")
            .GetAsync<ListResp<GrantedPermit>>();
    }

    public Task<IResp> ChangeRolePermits(long rid, ChangeRolePermitReq req)
    {
        return new PortalRequest($"/Permit/ChangeRolePermits?rid={rid}")
            .PostAsync<IResp>(req);
    }
}

