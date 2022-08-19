﻿using OSS.Common.Resp;
using OSS.Core.Client.Http;
using OSS.Core.Context;

namespace OSS.Core.Module.Portal.Client;

internal class PermitClient : IPermitOpenService
{
    /// <inheritdoc />
    public Task<ListResp<GrantedPermit>> GetCurrentUserPermits()
    {
        return new PortalRemoteRequest("/Permit/GetCurrentUserPermits")
            .GetAsync<ListResp<GrantedPermit>>();
    }

    /// <inheritdoc />
    public Task<IResp<FuncDataLevel>> CheckPermit(string funcCode, string sceneCode)
    {
        return new PortalRemoteRequest($"/Permit/CheckPermit?func_code={funcCode}&scene_code={sceneCode}")
            .PostAsync<IResp<FuncDataLevel>>();
    }

    /// <inheritdoc />
    public Task<ListResp<GrantedPermit>> GetPermitsByRoleId(long roleId)
    {
        return new PortalRemoteRequest($"/Permit/GetPermitsByRoleId?roleId={roleId}")
            .GetAsync<ListResp<GrantedPermit>>();
    }

    /// <inheritdoc />
    public Task<IResp> ChangeRolePermits(long rid, ChangeRolePermitReq req)
    {
        return new PortalRemoteRequest($"/Permit/ChangeRolePermits?rid={rid}")
            .PostAsync<IResp>(req);
    }
}

