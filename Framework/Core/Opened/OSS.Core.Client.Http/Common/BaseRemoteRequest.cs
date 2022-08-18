using System.Net.Http.Headers;
using OSS.Core.Context;
using OSS.Tools.Http;

namespace OSS.Core.Client.Http;

/// <summary>
///  客户端接口请求
/// </summary>
public class BaseRemoteRequest : OssHttpRequest
{
    internal readonly string target_module;

    /// <summary>
    /// 客户端接口请求
    /// </summary>
    /// <param name="targetModuleName">请求对应的模块名称</param>
    /// <param name="apiPath"></param>
    public BaseRemoteRequest(string targetModuleName, string apiPath) : base(apiPath)
    {
        target_module = targetModuleName;
    }

    private CoreAccessSecret _access;


    /// <inheritdoc />
    protected override async Task PrepareSendAsync()
    {
        _access     = await CoreClientHelper.AccessProvider.Get(target_module);
        address_url = string.Concat(_access.api_domain, address_url);
    }

    /// <inheritdoc />
    protected override Task OnSendingAsync(HttpRequestMessage r)
    {
        var ticket = CoreContext.App.Identity.ToTicket(_access.access_key, _access.access_secret,
            CoreContext.App.Self.AppVersion);

        r.Headers.Add(CoreClientHelper.HeaderName, ticket);
        r.Headers.Add("Accept", "application/json");

        if (r.Content != null)
        {
            r.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/json") { CharSet = "UTF-8" };
        }

        return Task.CompletedTask;
    }

    
}