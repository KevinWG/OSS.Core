using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace TM.Module.Product.Client;

internal class SpuHttpClient : ISpuOpenService
{
     private const string apiEntityPath = "/Product";

    /// <inheritdoc />
    public Task<TokenPageListResp<SpuMo>> MSearch(SearchReq req)
    {
          var apiPath = string.Concat(apiEntityPath , "/MSearch");

          return new ProductRemoteReq(apiPath)
            .PostAsync<TokenPageListResp<SpuMo>>(req);
    }

    /// <inheritdoc />
    public Task<Resp<SpuMo>> Get(long id)
    {
          var apiPath = string.Concat(apiEntityPath , "/Get?id=",id);

          return new ProductRemoteReq(apiPath).GetAsync<Resp<SpuMo>>();
    }

    /// <inheritdoc />
    public Task<Resp<SpuMo>> GetUseable(long id)
    {
          var apiPath = string.Concat(apiEntityPath , "/GetUseable?id=",id);

          return new ProductRemoteReq(apiPath).GetAsync<Resp<SpuMo>>();
    }
    
    /// <inheritdoc />
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
          var apiPath = string.Concat(apiEntityPath , "/SetUseable?pass_token=",pass_token,"&flag=",flag);

          return new ProductRemoteReq(apiPath).PostAsync<Resp>();
    }

    /// <inheritdoc />
    public Task<LongResp> Add(AddSpuReq req)
    {
          var apiPath = string.Concat(apiEntityPath , "/Add");

          return new ProductRemoteReq(apiPath).PostAsync<LongResp>(req);
    }
}

