using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace TM.WMS.Client;

internal class MCategoryHttpClient : IMCategoryOpenService
{
    private const string apiEntityPath = "/MCategory";

    /// <inheritdoc />
    public Task<Resp<MCategoryMo>> Get(long id)
    {
        var apiPath = string.Concat(apiEntityPath, "/Get?id=", id);
        return new WMSRemoteReq(apiPath)
            .GetAsync<Resp<MCategoryMo>>();
    }

    /// <inheritdoc />
    public Task<ListResp<MCategoryTreeItem>> AllCategories()
    {
        var apiPath = string.Concat(apiEntityPath, "/AllCategories");
        return new WMSRemoteReq(apiPath)
            .GetAsync<ListResp<MCategoryTreeItem>>();
    }


  

    /// <inheritdoc />
    public Task<LongResp> Add(AddCategoryReq req)
    {
        var apiPath = string.Concat(apiEntityPath, "/Add");
        return new WMSRemoteReq(apiPath)
            .PostAsync<LongResp>(req);
    }

    /// <inheritdoc />
    public Task<Resp> SetUseable(long id, ushort flag)
    {
        var apiPath = string.Concat(apiEntityPath, "/SetUseable?id=", id, "&flag=", flag);
        return new WMSRemoteReq(apiPath)
            .PostAsync<Resp>();
    }

    /// <inheritdoc />
    public Task<Resp> UpdateName(long id, UpdateMCategoryNameReq req)
    {
        var apiPath = string.Concat(apiEntityPath, "/UpdateName?id=", id);
        return new WMSRemoteReq(apiPath)
            .PostAsync<Resp>(req);
    }


    ///// <inheritdoc />
    //public Task<IResp<MCategoryMo>> GetUseable(long id)
    //{
    //    var apiPath = string.Concat(apiEntityPath, "/GetUseable?id=", id);
    //    return new WMSRemoteReq(apiPath)
    //        .GetAsync<IResp<MCategoryMo>>();
    //}

    ///// <inheritdoc />
    //public Task<PageListResp<MCategoryMo>> MSearch(SearchReq req)
    //{
    //    var apiPath = string.Concat(apiEntityPath, "/MSearch");
    //    return new WMSRemoteReq(apiPath)
    //        .PostAsync<PageListResp<MCategoryMo>>();
    //}

    /// <inheritdoc />
    public Task<Resp> UpdateOrder(long id, int order)
    {
        var apiPath = string.Concat(apiEntityPath, "/UpdateOrder?id=", id, "&order=", order);
        return new WMSRemoteReq(apiPath)
            .PostAsync<Resp>();
    }
}
