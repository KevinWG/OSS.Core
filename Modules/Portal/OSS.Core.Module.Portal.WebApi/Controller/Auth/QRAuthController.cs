using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OSS.Core.Module.Portal;

/// <summary>
///  小程序登录授权接口
/// </summary>
[AllowAnonymous]
public class QRAuthController : BasePortalController,IQRAuthOpenService
{
    private static readonly QRAuthService _service = new QRAuthService();

    /// <summary>
    ///  获取二维码登录的内容
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    [HttpGet]
    public Task<GetPortalQRResp> GetQRCode(PortalType platform)
    {
        return _service.GetQRCode(platform);
    }
    
    /// <summary>
    ///   查询用户登录状态并返回登录信息
    /// </summary>
    /// <param name="portal_ticket">二维码的登录票据</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PortalQRTokenResp> AskProgress(string portal_ticket)
    {
        var tokenResp = await _service.AskProgress(portal_ticket);

        PortalWebHelper.SetCookie(Response, tokenResp.token, true);

        return tokenResp;
    }

    /// <summary>
    ///   查询用户登录状态并返回登录信息
    /// </summary>
    /// <param name="portal_ticket">二维码的登录票据</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PortalQRTokenResp> AskAdminProgress(string portal_ticket)
    {
        var tokenResp = await _service.AskAdminProgress(portal_ticket);

        PortalWebHelper.SetCookie(Response, tokenResp.token,false);

        return tokenResp;
    }


}