using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client;

internal class SettingHttpClient : ISettingOpenService
{
    public Task<IResp> SaveAuthSetting(AuthSetting setting)
    {
        return new PortalRemoteReq("/Setting/SaveAuthSetting")
            .PostAsync<IResp>(setting);
    }

    public Task<IResp<AuthSetting>> GetAuthSetting()
    {
        return new PortalRemoteReq("/Setting/GetAuthSetting")
            .GetAsync<IResp<AuthSetting>>();//<IResp>(setting);
    }
}