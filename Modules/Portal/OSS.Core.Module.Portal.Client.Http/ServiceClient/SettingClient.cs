using OSS.Common.Resp;

namespace OSS.Core.Module.Portal.Client.Http;

internal class SettingClient : IOpenedSettingService
{
    public Task<IResp> SaveAuthSetting(AuthSetting setting)
    {
        throw new NotImplementedException();
    }

    public Task<IResp<AuthSetting>> GetAuthSetting()
    {
        throw new NotImplementedException();
    }
}