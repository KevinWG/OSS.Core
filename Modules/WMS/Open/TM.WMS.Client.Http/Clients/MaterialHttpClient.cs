using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace TM.WMS.Client;

internal class MaterialHttpClient : IMaterialOpenService
{
     private const string apiEntityPath = "/Material";

    /// <inheritdoc />
    public Task<PageListResp<MaterialView>> MSearch(SearchReq req)
    {
          var apiPath = string.Concat(apiEntityPath , "/MSearch");

          return new WMSRemoteReq(apiPath)
            .PostAsync<PageListResp<MaterialView>>(req);
    }

    /// <inheritdoc />
    public Task<Resp<MaterialView>> Get(long id)
    {
          var apiPath = string.Concat(apiEntityPath , "/Get?id=",id);

          return new WMSRemoteReq(apiPath).GetAsync<Resp<MaterialView>>();
    }

    /// <inheritdoc />
    public Task<Resp<MaterialView>> GetUseable(long id)
    {
          var apiPath = string.Concat(apiEntityPath , "/GetUseable?id=",id);

          return new WMSRemoteReq(apiPath).GetAsync<Resp<MaterialView>>();
    }
    
    /// <inheritdoc />
    public Task<Resp> SetUseable(long id, ushort flag)
    {
          var apiPath = string.Concat(apiEntityPath , "/SetUseable?id=",id,"&flag=",flag);

          return new WMSRemoteReq(apiPath).PostAsync<Resp>();
    }

    /// <inheritdoc />
    public Task<LongResp> Add(AddMaterialReq req)
    {
          var apiPath = string.Concat(apiEntityPath , "/Add");

          return new WMSRemoteReq(apiPath).PostAsync<LongResp>(req);
    }

    public Task<Resp> Update(long id, UpdateMaterialReq req)
    {
        var apiPath = string.Concat(apiEntityPath, "/Update?id=",id);

        return new WMSRemoteReq(apiPath).PostAsync<Resp>(req);
    }
}

