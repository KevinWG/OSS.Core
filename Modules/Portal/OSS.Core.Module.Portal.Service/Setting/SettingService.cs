using OSS.Common.Resp;
using OSS.Tools.DirConfig;

namespace OSS.Core.Module.Portal;

public class SettingService : ISettingCommonService
{
    /// <inheritdoc />
    public async Task<IResp> SaveAuthSetting(AuthSetting setting)
    {
        var isOk = await DirConfigHelper.SetDirConfig(PortalConst.DirConfigKeys.auth_setting, setting);
        return isOk ? Resp.DefaultSuccess : new Resp(RespCodes.OperateFailed, "保存授权信息配置失败!");
    }

    /// <inheritdoc />
    public async Task<IResp<AuthSetting>> GetAuthSetting()
    {
        var config = await DirConfigHelper.GetDirConfig<AuthSetting>(PortalConst.DirConfigKeys.auth_setting);

        var res = new Resp<AuthSetting>();

        return config==null ? res.WithResp(RespCodes.OperateObjectNull,"授权配置信息尚未设置!") : res.WithData(config);
    }
}