using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace TM.WMS.Client;

internal class BatchHttpClient : IBatchOpenService
{
     private const string apiEntityPath = "/Batch";

    /// <inheritdoc />
    public Task<TokenPageListResp<BatchMo>> MSearch(SearchReq req)
    {
          var apiPath =$"{apiEntityPath}/MSearch";
          return new WMSRemoteReq(apiPath)
            .PostAsync<TokenPageListResp<BatchMo>>(req);
    }

    /// <inheritdoc />
    public Task<Resp<BatchMo>> Get(long id)
    {
        var apiPath = $"{apiEntityPath}/Get?id={id}";
        return new WMSRemoteReq(apiPath).GetAsync<Resp<BatchMo>>();
    }

    /// <inheritdoc />
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
          var apiPath =$"{apiEntityPath}/SetUseable?pass_token={pass_token}&flag={flag}";

          return new WMSRemoteReq(apiPath).PostAsync<Resp>();
    }

    /// <inheritdoc />
    public Task<LongResp> Add(AddBatchCodeReq req)
    {
          var apiPath =$"{apiEntityPath}/Add";
          return new WMSRemoteReq(apiPath).PostAsync<LongResp>(req);
    }
}

