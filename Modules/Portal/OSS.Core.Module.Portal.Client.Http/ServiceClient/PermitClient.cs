using OSS.Common.Resp;
using OSS.Core.Client.Http;
using OSS.Core.Context;

namespace OSS.Core.Module.Portal.Client;

internal class PermitClient : IOpenedPermitService
{
    /// <inheritdoc />
    public Task<ListResp<GrantedPermit>> GetCurrentUserPermits()
    {
        return new PortalRequest("/Permit/GetCurrentUserPermits")
            .GetAsync<ListResp<GrantedPermit>>();
    }

    /// <inheritdoc />
    public Task<IResp<FuncDataLevel>> CheckPermit(string funcCode, string sceneCode)
    {
        return new PortalRequest($"/Permit/CheckPermit?func_code={funcCode}&scene_code={sceneCode}")
            .PostAsync<IResp<FuncDataLevel>>();
    }

    /// <inheritdoc />
    public Task<ListResp<GrantedPermit>> GetPermitsByRoleId(long roleId)
    {
        return new PortalRequest($"/Permit/GetPermitsByRoleId?roleId={roleId}")
            .GetAsync<ListResp<GrantedPermit>>();
    }

    /// <inheritdoc />
    public Task<IResp> ChangeRolePermits(long rid, ChangeRolePermitReq req)
    {
        return new PortalRequest($"/Permit/ChangeRolePermits?rid={rid}")
            .PostAsync<IResp>(req);
    }
}

