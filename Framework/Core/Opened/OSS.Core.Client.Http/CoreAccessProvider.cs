using Microsoft.Extensions.Configuration;
using OSS.Tools.Config;
using System.Data;

namespace OSS.Core.Client.Http;

/// <summary>
///  接口请求秘钥提供者
/// </summary>
internal class CoreAccessProvider : ICoreAccessProvider
{
    public Task<CoreAccessSecret> Get(string moduleName)
    {
        var secret = new CoreAccessSecret();
        ConfigHelper.Configuration.GetSection(string.Concat("Client:", moduleName)).Bind(secret);

        if (string.IsNullOrEmpty(secret.access_secret))
            throw new NoNullAllowedException($"未能找到 Client:{moduleName} 配置节点信息!");

        return Task.FromResult(secret);
    }
}