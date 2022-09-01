namespace OSS.Core.Module.Portal;

public interface IQRAuthOpenService
{
    /// <summary>
    ///  获取二维码登录的内容
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    Task<GetPortalQRResp> GetQRCode(PortalType platform);

    /// <summary>
    ///   查询用户登录状态并返回登录信息
    /// </summary>
    /// <param name="portalTicket"></param>
    /// <returns></returns>
    Task<PortalQRTokenResp> AskProgress(string portalTicket);

    /// <summary>
    ///   查询管理员登录状态并返回登录信息
    /// </summary>
    /// <param name="portalTicket"></param>
    /// <returns></returns>
    Task<PortalQRTokenResp> AskAdminProgress(string portalTicket);
}