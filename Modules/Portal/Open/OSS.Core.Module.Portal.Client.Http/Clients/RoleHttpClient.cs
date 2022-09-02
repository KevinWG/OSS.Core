using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client;

internal class RoleHttpClient : IRoleOpenService
{
    public Task<PageListResp<RoleMo>> Search(SearchReq req)
    {
        return new PortalRemoteRequest("/Role/Search" )
            .PostAsync<PageListResp<RoleMo>>(req);
    }

    public Task<IResp> Add(AddRoleReq req)
    {
        return new PortalRemoteRequest("/Role/Add")
            .PostAsync<IResp>(req);
    }

    public Task<IResp> UpdateName(long id, AddRoleReq req)
    {
        return new PortalRemoteRequest("/Role/UpdateName?id="+ id)
            .PostAsync<IResp>(req);
    }

    public Task<IResp> Active(long id)
    {
        return new PortalRemoteRequest("/Role/Active?id=" + id)
            .PostAsync<IResp>();
    }

    public Task<IResp> UnActive(long id)
    {
        return new PortalRemoteRequest("/Role/UnActive?id=" + id)
            .PostAsync<IResp>();
    }

    public Task<ListResp<RoleMo>> GetUserRoles(long userId)
    {
        return new PortalRemoteRequest("/Role/GetUserRoles?u_id=" + userId)
            .GetAsync<ListResp<RoleMo>>();
    }

    public Task<IResp> UserBind(AddRoleUserReq req)
    {
        return new PortalRemoteRequest("/Role/UserBind")
            .PostAsync<IResp>(req);
    }

    public Task<IResp> DeleteUserBind(long userId, long roleId)
    {
        return new PortalRemoteRequest(string.Concat("/Role/DeleteUserBind?u_id=", userId, "&r_id=", roleId))
            .PostAsync<IResp>();
    }
}

internal class UserHttpClient : IUserOpenService
{
    public Task<IResp> ChangeMyBasic(UpdateUserBasicReq req)
    {
        return new PortalRemoteRequest("/User/ChangeMyBasic")
            .PostAsync<IResp>(req);
    }

    public Task<PageListResp<UserBasicMo>> SearchUsers(SearchReq req)
    {
        return new PortalRemoteRequest("/User/SearchUsers")
            .PostAsync<PageListResp<UserBasicMo>>(req);
    }

    public Task<Resp<UserBasicMo>> Get(long id)
    {
        return new PortalRemoteRequest("/User/Get?id="+ id)
            .GetAsync<Resp<UserBasicMo>>();
    }

    public Task<IResp> Lock(long id)
    {
        return new PortalRemoteRequest("/User/Lock?id=" + id)
            .PostAsync<IResp>();
    }

    public Task<IResp> UnLock(long id)
    {
        return new PortalRemoteRequest("/User/UnLock?id=" + id)
            .PostAsync<IResp>();
    }
}