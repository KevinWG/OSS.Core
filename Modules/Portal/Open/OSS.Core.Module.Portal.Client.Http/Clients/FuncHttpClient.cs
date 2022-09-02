using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client;

internal class FuncHttpClient : IFuncOpenService
{
    public Task<IResp> CheckFuncCode(string code)
    {
        return new PortalRemoteRequest("/Func/CheckFuncCode?code=" + code)
            .PostAsync<IResp>();
    }

    public Task<ListResp<FuncMo>> GetAllFuncItems()
    {
        return new PortalRemoteRequest("/Func/GetAllFuncItems")
            .GetAsync<ListResp<FuncMo>>();
    }

    public Task<IResp> AddFuncItem(AddFuncItemReq req)
    {
        return new PortalRemoteRequest("/Func/AddFuncItem")
            .PostAsync<IResp>(req);
    }

    public Task<IResp> ChangeFuncItem(string code, ChangeFuncItemReq req)
    {
        return new PortalRemoteRequest("/Func/ChangeFuncItem?code="+ code)
            .PostAsync<IResp>(req);
    }
}