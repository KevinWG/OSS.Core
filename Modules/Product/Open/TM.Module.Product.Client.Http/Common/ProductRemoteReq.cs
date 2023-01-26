
using OSS.Core.Client.Http;

namespace TM.Module.Product.Client;

/// <summary>
///  Product 模块客户端请求
/// </summary>
internal class ProductRemoteReq: BaseRemoteRequest
{
    public ProductRemoteReq(string apiPath) : base("Product", apiPath)
    {
    }

    protected override Task PrepareSendAsync()
    {
        api_path = string.Concat("/",target_module, api_path);
        return base.PrepareSendAsync();
    }
}
