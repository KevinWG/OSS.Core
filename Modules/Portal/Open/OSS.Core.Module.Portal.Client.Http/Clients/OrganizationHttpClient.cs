using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Portal.Client;

internal class OrganizationHttpClient : IOrganizationOpenService
{
     private const string apiEntityPath = "/Organization";

    /// <inheritdoc />
    public Task<TokenPageListResp<OrganizationMo>> MSearch(SearchReq req)
    {
          var apiPath = string.Concat(apiEntityPath , "/MSearch");

          return new PortalRemoteReq(apiPath)
            .PostAsync<TokenPageListResp<OrganizationMo>>(req);
    }

    public Task<PageListResp<OrganizationMo>> ComSearch(SearchReq req)
    {
        var apiPath = string.Concat(apiEntityPath, "/ComSearch");

        return new PortalRemoteReq(apiPath)
            .PostAsync<PageListResp<OrganizationMo>>(req);
    }


    /// <inheritdoc />
    public Task<Resp<OrganizationMo>> Get(long id)
    {
          var apiPath = string.Concat(apiEntityPath , "/Get?id=",id);

          return new PortalRemoteReq(apiPath).GetAsync<Resp<OrganizationMo>>();
    }

    /// <inheritdoc />
    public Task<Resp<OrganizationMo>> GetUseable(long id)
    {
          var apiPath = string.Concat(apiEntityPath , "/GetUseable?id=",id);

          return new PortalRemoteReq(apiPath).GetAsync<Resp<OrganizationMo>>();
    }
    
    /// <inheritdoc />
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
          var apiPath = string.Concat(apiEntityPath , "/SetUseable?pass_token=",pass_token,"&flag=",flag);

          return new PortalRemoteReq(apiPath).PostAsync<Resp>();
    }

    /// <inheritdoc />
    public Task<LongResp> Add(AddOrganizationReq req)
    {
          var apiPath = string.Concat(apiEntityPath , "/Add");

          return new PortalRemoteReq(apiPath).PostAsync<LongResp>(req);
    }
}

