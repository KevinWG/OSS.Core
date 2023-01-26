using OSS.Common.Resp;
using OSS.Core.Client.Http;

namespace TM.WMS.Client;

internal class UnitHttpClient : IUnitOpenService
{
    private const string apiEntityPath = "/Unit";


    public Task<ListResp<UnitView>> All()
    {
        var apiPath = string.Concat(apiEntityPath, "/All");

        return new WMSRemoteReq(apiPath)
            .GetAsync<ListResp<UnitView>>();
    }

    /// <inheritdoc />
    public Task<LongResp> Add(AddUnitReq req)
    {
        var apiPath = string.Concat(apiEntityPath, "/Add");

        return new WMSRemoteReq(apiPath).PostAsync<LongResp>(req);
    }
}

