using OSS.Common.Resp;

namespace OSS.Core.Module.Portal;

public class PortalQRTokenResp : PortalTokenResp
{
    /// <summary>
    ///  扫码的状态
    /// </summary>
    public QRProgress progress { get; set; }
}


public static class PortalQRTokenRespExtension
{
    public static PortalQRTokenResp ToQRToken(this PortalTokenResp res)
    {
        return new PortalQRTokenResp()
        {

            data     = res.data,
            token    = res.token,
            progress = QRProgress.ProcessEnd

        }.WithResp(res);
    }
}