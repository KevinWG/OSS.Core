
using OSS.Core.Client.Http;

namespace TM.WMS.Client;

/// <summary>
///  WMS 模块客户端请求
/// </summary>
internal class WMSRemoteReq: BaseRemoteRequest
{
    public WMSRemoteReq(string apiPath) : base("WMS", apiPath)
    {
    }

    protected override Task PrepareSendAsync()
    {
        api_path = string.Concat("/",target_module, api_path);
        return base.PrepareSendAsync();
    }
}
