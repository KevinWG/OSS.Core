using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client;

internal class RoleHttpClient : IRoleOpenService
{
    public Task<PageListResp<RoleMo>> Search(SearchReq req)
    {
        return new PortalRemoteReq("/Role/Search" )
            .PostAsync<PageListResp<RoleMo>>(req);
    }

    public Task<Resp> Add(AddRoleReq req)
    {
        return new PortalRemoteReq("/Role/Add")
            .PostAsync<Resp>(req);
    }

    public Task<Resp> UpdateName(long id, AddRoleReq req)
    {
        return new PortalRemoteReq("/Role/UpdateName?id="+ id)
            .PostAsync<Resp>(req);
    }

    public Task<Resp> Active(long id)
    {
        return new PortalRemoteReq("/Role/Active?id=" + id)
            .PostAsync<Resp>();
    }

    public Task<Resp> UnActive(long id)
    {
        return new PortalRemoteReq("/Role/UnActive?id=" + id)
            .PostAsync<Resp>();
    }

    public Task<ListResp<RoleMo>> GetUserRoles(long userId)
    {
        return new PortalRemoteReq("/Role/GetUserRoles?u_id=" + userId)
            .GetAsync<ListResp<RoleMo>>();
    }

    public Task<Resp> UserBind(AddRoleUserReq req)
    {
        return new PortalRemoteReq("/Role/UserBind")
            .PostAsync<Resp>(req);
    }

    public Task<Resp> DeleteUserBind(long userId, long roleId)
    {
        return new PortalRemoteReq(string.Concat("/Role/DeleteUserBind?u_id=", userId, "&r_id=", roleId))
            .PostAsync<Resp>();
    }
}

internal class UserHttpClient : IUserOpenService
{
    public Task<Resp> ChangeMyBasic(UpdateUserBasicReq req)
    {
        return new PortalRemoteReq("/User/ChangeMyBasic")
            .PostAsync<Resp>(req);
    }

    public Task<PageListResp<UserBasicMo>> SearchUsers(SearchReq req)
    {
        return new PortalRemoteReq("/User/SearchUsers")
            .PostAsync<PageListResp<UserBasicMo>>(req);
    }

    public Task<Resp<UserBasicMo>> Get(long id)
    {
        return new PortalRemoteReq("/User/Get?id="+ id)
            .GetAsync<Resp<UserBasicMo>>();
    }

    public Task<Resp> Lock(long id)
    {
        return new PortalRemoteReq("/User/Lock?id=" + id)
            .PostAsync<Resp>();
    }

    public Task<Resp> UnLock(long id)
    {
        return new PortalRemoteReq("/User/UnLock?id=" + id)
            .PostAsync<Resp>();
    }
}