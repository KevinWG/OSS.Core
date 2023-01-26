using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace TM.WMS.Client;

internal class StockApplyHttpClient : IStockApplyOpenService
{
    /// <inheritdoc />
    public Task<TokenPageListResp<StockApplyMo>> MSearch(SearchReq req)
    {
          return new WMSRemoteReq("/StockApply/MSearch")
            .PostAsync<TokenPageListResp<StockApplyMo>>(req);
    }

    /// <inheritdoc />
    public Task<Resp<StockApplyMo>> Get(long id)
    {
          return new WMSRemoteReq($"/StockApply/Get?id={id}")
            .GetAsync<Resp<StockApplyMo>>();
    }

    /// <inheritdoc />
    public Task<Resp<StockApplyMo>> GetUseable(long id)
    {
          return new WMSRemoteReq($"/StockApply/GetUseable?id={id}")
            .GetAsync<Resp<StockApplyMo>>();
    }
    
    /// <inheritdoc />
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
          return new WMSRemoteReq($"/StockApply/SetUseable?pass_token={pass_token}&flag={flag}")
            .PostAsync<Resp>();
    }

    /// <inheritdoc />
    public Task<LongResp> Add(AddStockApplyReq req)
    {
          return new WMSRemoteReq("/StockApply/Add")
            .PostAsync<LongResp>(req);
    }

    public Task<Resp> Edit(string pass_token, AddStockApplyReq req)
    {
        return new WMSRemoteReq("/StockApply/Edit?pass_token="+pass_token)
            .PostAsync<Resp>(req);
    }
}

