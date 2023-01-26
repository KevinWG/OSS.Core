using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace TM.WMS.Client;

internal class WarehouseHttpClient : IWarehouseOpenService
{
    private const string apiEntityPath = "/Warehouse";

    public Task<ListResp<WarehouseFlatItem>> AllUseable()
    {
        var apiPath = string.Concat(apiEntityPath, "/AllUseable");

        return new WMSRemoteReq(apiPath)
            .GetAsync<ListResp<WarehouseFlatItem>>();
    }

    public Task<ListResp<WarehouseFlatItem>> All()
    {
        var apiPath = string.Concat(apiEntityPath, "/All");

        return new WMSRemoteReq(apiPath)
            .GetAsync<ListResp<WarehouseFlatItem>>();
    }

    public Task<Resp<WarehouseFlatItem>> GetUseableFlatItem(long id)
    {
        var apiPath = string.Concat(apiEntityPath, "/GetUseableFlatItem?id=", id);
        return new WMSRemoteReq(apiPath)
            .GetAsync<Resp<WarehouseFlatItem>>();
    }

    public Task<Resp> SetUseable(long id, ushort flag)
    {
        var apiPath = string.Concat(apiEntityPath, "/SetUseable?id=", id, "&flag=", flag);

        return new WMSRemoteReq(apiPath).PostAsync<Resp>();
    }

    /// <inheritdoc />
    public Task<LongResp> Add(AddWarehouseReq req)
    {
        var apiPath = string.Concat(apiEntityPath, "/Add");

        return new WMSRemoteReq(apiPath).PostAsync<LongResp>(req);
    }

    public Task<Resp> Update(long id, UpdateWarehouseReq req)
    {
        var apiPath = string.Concat(apiEntityPath, "/Update?id=",id);

        return new WMSRemoteReq(apiPath).PostAsync<Resp>(req);
    }

    public Task<LongResp> AddArea(AddAreaReq req)
    {
        var apiPath = string.Concat(apiEntityPath, "/AddArea");

        return new WMSRemoteReq(apiPath).PostAsync<LongResp>(req);
    }

    public Task<Resp> UpdateArea(long id, UpdateAreaReq req)
    {
        var apiPath = string.Concat(apiEntityPath, "/UpdateArea?id=",id);

        return new WMSRemoteReq(apiPath).PostAsync<Resp>(req);
    }

    public Task<Resp> CheckAreaCode(CheckAreaCodeReq req)
    {
        var apiPath = string.Concat(apiEntityPath, "/CheckAreaCode");

        return new WMSRemoteReq(apiPath).PostAsync<Resp>(req);
    }

    public Task<ListResp<WareAreaMo>> AreaList(long w_id)
    {
        var apiPath = string.Concat(apiEntityPath, "/AreaList");

        return new WMSRemoteReq(apiPath)
            .GetAsync<ListResp<WareAreaMo>>();
    }

    public Task<Resp> SetAreaUseable(long id, ushort flag)
    {
        var apiPath = string.Concat(apiEntityPath, "/SetAreaUseable?id=", id, "&flag=", flag);

        return new WMSRemoteReq(apiPath).PostAsync<Resp>();
    }

    public Task<Resp> SetAreaTradeFlag(long id, TradeFlag flag)
    {
        var apiPath = string.Concat(apiEntityPath, "/SetAreaTradeFlag?id=", id, "&flag=", (int)flag);

        return new WMSRemoteReq(apiPath).PostAsync<Resp>();
    }
}

