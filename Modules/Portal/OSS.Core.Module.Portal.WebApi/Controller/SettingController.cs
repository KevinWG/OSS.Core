using Microsoft.AspNetCore.Mvc;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes;

namespace OSS.Core.Module.Portal;

/// <summary>
///   配置管理
/// </summary>
public class SettingController : BasePortalController, ISettingOpenService
{
    private static readonly SettingService _service = new();

    /// <summary>
    /// 保存授权配置信息
    /// </summary>
    /// <param name="setting"></param>
    /// <returns></returns>
    [HttpPost]
    [UserFuncMeta(PortalConst.FuncCodes.portal_auth_setting)]
    public Task<IResp> SaveAuthSetting([FromBody]AuthSetting setting)
    {
        return _service.SaveAuthSetting(setting);
    }

    /// <summary>
    ///  获取授权配置信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [UserFuncMeta(PortalConst.FuncCodes.portal_auth_setting)]
    public Task<IResp<AuthSetting>> GetAuthSetting()
    {
       return _service.GetAuthSetting();
    }
}