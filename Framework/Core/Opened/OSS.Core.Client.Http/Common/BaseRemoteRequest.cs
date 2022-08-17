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

    /// <inheritdoc />
    protected override async Task OnSendingAsync(HttpRequestMessage r)
    {
        var accessSecret = await CoreClientHelper.AccessProvider.Get(target_module);

        address_url = string.Concat(accessSecret.api_domain, address_url);

        var ticket = CoreContext.App.Identity.ToTicket(accessSecret.access_key, accessSecret.access_secret,
            CoreContext.App.Self.AppVersion);

        r.Headers.Add(CoreClientHelper.HeaderName, ticket);
        r.Headers.Add("Accept", "application/json");

        if (r.Content != null)
        {
            r.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/json") { CharSet = "UTF-8" };
        }
    }

    
}