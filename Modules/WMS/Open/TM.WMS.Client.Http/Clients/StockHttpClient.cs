using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace TM.WMS.Client;

internal class StockHttpClient : IStockOpenService
{
    private const string apiEntityPath = "/Stock";

    ///// <inheritdoc />
    //public Task<TokenPageListResp<AreaStockMo>> MSearch(SearchReq req)
    //{
    //    var apiPath = string.Concat(apiEntityPath, "/MSearch");

    //    return new WMSRemoteReq(apiPath)
    //        .PostAsync<TokenPageListResp<AreaStockMo>>(req);
    //}

    //public Task<Resp<AreaStockMo>> Get(long m_id, long warehouse_id, long area_id, string batch_code)
    //{
    //    var apiPath =
    //        $"{apiEntityPath}/Get?m_id={m_id}&warehouse_id={warehouse_id}&area_id={area_id}&batch_code={batch_code}";
    //    return new WMSRemoteReq(apiPath).GetAsync<Resp<AreaStockMo>>();
    //}



    public Task<PageListResp<MaterialStockView>> MSearchUnion(SearchReq req)
    {
        var apiPath = string.Concat(apiEntityPath, "/MSearchUnion");

        return new WMSRemoteReq(apiPath)
            .PostAsync<PageListResp<MaterialStockView>>(req);
    }

    public Task<ListResp<MaterialStockCount>> GetStockList(GetStockListReq req)
    {
        var apiPath = string.Concat(apiEntityPath, "/GetStockList");

        return new WMSRemoteReq(apiPath)
            .PostAsync<ListResp<MaterialStockCount>>(req);
    }

    public Task<PageListResp<AreaStockView>> SearchArea(SearchReq req)
    {
        var apiPath = string.Concat(apiEntityPath, "/SearchArea");

        return new WMSRemoteReq(apiPath)
            .PostAsync<PageListResp<AreaStockView>>(req);
    }

    /// <inheritdoc />
    public Task<Resp> StockChange(StockChangeReq req)
    {
        var apiPath = string.Concat(apiEntityPath, "/StockChange");

        return new WMSRemoteReq(apiPath)
            .PostAsync<Resp>(req);
    }
}

