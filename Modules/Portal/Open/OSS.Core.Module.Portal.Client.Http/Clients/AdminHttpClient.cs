using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client;

internal class AdminHttpClient : IAdminOpenService
{
    public Task<Resp> ChangeMyAvatar(string avatar)
    {
        return new PortalRemoteReq("/Admin/ChangeMyAvatar?avatar="+ avatar)
            .PostAsync<Resp>(); 
    }

    public Task<Resp> ChangeMyName(string name)
    {
        return new PortalRemoteReq("/Admin/ChangeMyName?name=" + name)
            .PostAsync<Resp>();
    }

    public Task<Resp<long>> Add(AddAdminReq admin)
    {
        return new PortalRemoteReq("/Admin/Add")
            .PostAsync<Resp<long>>(admin);
    }
    
    public Task<PageListResp<AdminInfoMo>> SearchAdmins(SearchReq req)
    {
        return new PortalRemoteReq("/Admin/SearchAdmins")
            .PostAsync<PageListResp<AdminInfoMo>>(req);
    }

    public Task<Resp> Lock(long uid)
    {
        return new PortalRemoteReq("/Admin/Lock?uid="+ uid)
            .PostAsync<Resp>();
    }

    public Task<Resp> UnLock(long uid)
    {
        return new PortalRemoteReq("/Admin/UnLock?uid=" + uid)
            .PostAsync<Resp>();
    }

    public Task<Resp> SetAdminType(long uId, AdminType adminType)
    {
        return new PortalRemoteReq(string.Concat("/Admin/SetAdminType?uid=" , uId, "&admin_type=",(int)adminType))
            .PostAsync<Resp>();
    }
}