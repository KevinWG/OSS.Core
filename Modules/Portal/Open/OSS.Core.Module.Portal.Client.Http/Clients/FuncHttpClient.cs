using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client;

internal class FuncHttpClient : IFuncOpenService
{
    public Task<IResp> CheckFuncCode(string code)
    {
        return new PortalRemoteReq("/Func/CheckFuncCode?code=" + code)
            .PostAsync<IResp>();
    }

    public Task<ListResp<FuncMo>> GetAllFuncItems()
    {
        return new PortalRemoteReq("/Func/GetAllFuncItems")
            .GetAsync<ListResp<FuncMo>>();
    }

    public Task<IResp> AddFuncItem(AddFuncItemReq req)
    {
        return new PortalRemoteReq("/Func/AddFuncItem")
            .PostAsync<IResp>(req);
    }

    public Task<IResp> ChangeFuncItem(string code, ChangeFuncItemReq req)
    {
        return new PortalRemoteReq("/Func/ChangeFuncItem?code="+ code)
            .PostAsync<IResp>(req);
    }
}