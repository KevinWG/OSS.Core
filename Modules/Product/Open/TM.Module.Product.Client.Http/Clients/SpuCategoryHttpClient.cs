using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace TM.Module.Product.Client;

internal class SpuCategoryHttpClient : ISpuCategoryOpenService
{
     private const string apiEntityPath = "/ProductCategory";

     /// <inheritdoc />
     public Task<Resp<SpuCategoryMo>> Get(long id)
     {
         var apiPath = string.Concat(apiEntityPath, "/Get?id=", id);
         return new ProductRemoteReq(apiPath)
             .GetAsync<Resp<SpuCategoryMo>>();
     }


     /// <inheritdoc />
     public Task<PageListResp<SpuCategoryMo>> MSearch(SearchReq req)
     {
         var apiPath = string.Concat(apiEntityPath, "/MSearch");
         return new ProductRemoteReq(apiPath)
             .PostAsync<PageListResp<SpuCategoryMo>>();
     }

     /// <inheritdoc />
     public Task<ListResp<SpuCategoryTreeItem>> AllCategories()
     {
        var apiPath = string.Concat(apiEntityPath, "/AllCategories");
        return new ProductRemoteReq(apiPath)
            .GetAsync<ListResp<SpuCategoryTreeItem>>();
    }

     /// <inheritdoc />
     public Task<IResp<SpuCategoryMo>> GetUseable(long id)
     {
         var apiPath = string.Concat(apiEntityPath, "/GetUseable?id=", id);
         return new ProductRemoteReq(apiPath)
             .GetAsync<IResp<SpuCategoryMo>>();
     }

     /// <inheritdoc />
     public Task<LongResp> Add(AddSpuCategoryReq req)
     {
         var apiPath = string.Concat(apiEntityPath, "/Add");
         return new ProductRemoteReq(apiPath)
             .PostAsync<LongResp>(req);
     }

     /// <inheritdoc />
     public Task<Resp> SetUseable(long id, ushort flag)
     {
         var apiPath = string.Concat(apiEntityPath, "/SetUseable?id=", id, "&flag=", flag);
         return new ProductRemoteReq(apiPath)
             .PostAsync<Resp>();
     }

     /// <inheritdoc />
     public Task<Resp> UpdateName(long id, UpdateSCNameReq req)
     {
         var apiPath = string.Concat(apiEntityPath, "/UpdateName?id=", id);
         return new ProductRemoteReq(apiPath)
             .PostAsync<Resp>(req);
     }

     /// <inheritdoc />
     public Task<Resp> UpdateOrder(long id, int order)
     {
         var apiPath = string.Concat(apiEntityPath, "/UpdateOrder?id=", id, "&order=", order);
         return new ProductRemoteReq(apiPath)
             .PostAsync<Resp>();
     }
}
