using OSS.Common.Resp;

namespace OSS.Core.Module.Portal;

public interface IOpenedSettingService
{
    /// <summary>
    ///  保存授权配置信息
    /// </summary>
    /// <param name="setting"></param>
    /// <returns></returns>
    Task<IResp> SaveAuthSetting(AuthSetting setting);

    /// <summary>
    /// 获取授权配置信息
    /// </summary>
    /// <returns></returns>
    Task<IResp<AuthSetting>> GetAuthSetting();
}