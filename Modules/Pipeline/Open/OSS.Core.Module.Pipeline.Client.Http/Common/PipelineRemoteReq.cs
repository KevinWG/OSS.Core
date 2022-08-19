
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Pipeline.Client;

/// <summary>
///  Pipeline 模块客户端请求
/// </summary>
internal class PipelineRemoteReq: BaseRemoteRequest
{
    public PipelineRemoteReq(string apiPath) : base("Pipeline", apiPath)
    {
    }

    protected override Task PrepareSendAsync()
    {
        api_path = string.Concat("/",target_module, api_path);
        return base.PrepareSendAsync();
    }
}
