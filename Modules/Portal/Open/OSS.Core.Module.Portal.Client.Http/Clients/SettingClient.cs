using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client;

internal class SettingClient : IOpenedSettingService
{
    public Task<IResp> SaveAuthSetting(AuthSetting setting)
    {
        return new PortalRemoteRequest("/Setting/SaveAuthSetting")
            .PostAsync<IResp>(setting);
    }

    public Task<IResp<AuthSetting>> GetAuthSetting()
    {
        return new PortalRemoteRequest("/Setting/GetAuthSetting")
            .GetAsync<IResp<AuthSetting>>();//<IResp>(setting);
    }
}
