using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client;

internal class AdminHttpClient : IAdminOpenService
{
    public Task<IResp> ChangeMyAvatar(string avatar)
    {
        return new PortalRemoteRequest("/Admin/ChangeMyAvatar?avatar="+ avatar)
            .PostAsync<IResp>(); 
    }

    public Task<IResp> ChangeMyName(string name)
    {
        return new PortalRemoteRequest("/Admin/ChangeMyName?name=" + name)
            .PostAsync<IResp>();
    }

    public Task<Resp<long>> Add(AddAdminReq admin)
    {
        return new PortalRemoteRequest("/Admin/Add")
            .PostAsync<Resp<long>>(admin);
    }

    public Task<PageListResp<AdminInfoMo>> SearchAdmins(SearchReq req)
    {
        return new PortalRemoteRequest("/Admin/SearchAdmins")
            .PostAsync<PageListResp<AdminInfoMo>>(req);
    }

    public Task<IResp> Lock(long uid)
    {
        return new PortalRemoteRequest("/Admin/Lock?uid="+ uid)
            .PostAsync<IResp>();
    }

    public Task<IResp> UnLock(long uid)
    {
        return new PortalRemoteRequest("/Admin/UnLock?uid=" + uid)
            .PostAsync<IResp>();
    }

    public Task<IResp> SetAdminType(long uId, AdminType adminType)
    {
        return new PortalRemoteRequest(string.Concat("/Admin/SetAdminType?uid=" , uId, "&admin_type=",(int)adminType))
            .PostAsync<IResp>();
    }
}