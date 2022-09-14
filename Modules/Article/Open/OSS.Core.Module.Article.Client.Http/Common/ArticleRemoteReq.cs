
using OSS.Core.Client.Http;

namespace OSS.Core.Module.Article.Client;

/// <summary>
///  Article 模块客户端请求
/// </summary>
internal class ArticleRemoteReq: BaseRemoteRequest
{
    public ArticleRemoteReq(string apiPath) : base("Article", apiPath)
    {
    }

    protected override Task PrepareSendAsync()
    {
        api_path = string.Concat("/",target_module, api_path);
        return base.PrepareSendAsync();
    }
}
