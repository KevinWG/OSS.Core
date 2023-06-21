using System.Net.Http.Headers;
using OSS.Core.Context;
using OSS.Tools.Http;

namespace OSS.Core.Client.Http;

/// <summary>
///  客户端接口请求
/// </summary>
public class BaseRemoteRequest : OssHttpRequest
{
    /// <summary>
    /// 模块名称
    /// </summary>
    protected internal string target_module { get; set; }
    /// <summary>
    ///  接口请求路径
    /// </summary>
    protected string api_path { get; set; }

    /// <summary>
    /// 客户端接口请求
    /// </summary>
    /// <param name="targetModuleName">请求对应的模块名称</param>
    /// <param name="apiPath"></param>
    public BaseRemoteRequest(string targetModuleName, string apiPath) //: base(apiPath)
    {
        target_module = targetModuleName;
        api_path = apiPath;
    }

    private CoreAccessSecret? _access;


    /// <inheritdoc />
    protected override async Task PrepareSendAsync()
    {
        _access     = await CoreClientHelper.AccessProvider.Get(target_module);
        address_url = string.Concat(_access.api_domain, api_path);
    }

    /// <inheritdoc />
    protected override Task OnSendingAsync(HttpRequestMessage r)
    {
        var appIdentity = CoreContext.App.Identity;

        var ticket = CoreContext.App.Identity.ToTicket(_access.access_key, _access.access_secret,
            CoreContext.App.Self.version, appIdentity.authorization);

        r.Headers.Add("Accept", "application/json");
        r.Headers.Add(CoreClientHelper.SignTicketHeaderName, ticket);
      
        if (!string.IsNullOrEmpty(appIdentity.authorization))
        {
            r.Headers.Add("Authorization", appIdentity.authorization);
        }

        if (r.Content != null)
        {
            r.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/json") { CharSet = "UTF-8" };
        }

        return Task.CompletedTask;
    }
}