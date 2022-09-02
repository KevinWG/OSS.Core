using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client;

internal class QRAuthHttpClient : IQRAuthOpenService
{
    public Task<GetPortalQRResp> GetQRCode(PortalType platform)
    {
        return new PortalRemoteRequest("/QRAuth/GetQRCode?platform="+(int)platform)
            .GetAsync<GetPortalQRResp>();
    }

    public Task<PortalQRTokenResp> AskProgress(string portalTicket)
    {
        return new PortalRemoteRequest("/QRAuth/AskProgress?portal_ticket=" + portalTicket)
            .GetAsync<PortalQRTokenResp>();
    }

    public Task<PortalQRTokenResp> AskAdminProgress(string portalTicket)
    {
        return new PortalRemoteRequest("/QRAuth/AskAdminProgress?portal_ticket=" + portalTicket)
            .GetAsync<PortalQRTokenResp>();
    }
}