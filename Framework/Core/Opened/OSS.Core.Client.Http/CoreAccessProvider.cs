using Microsoft.Extensions.Configuration;
using OSS.Common;
using OSS.Tools.Config;
using System.Data;

namespace OSS.Core.Client.Http;

/// <summary>
///  接口请求秘钥提供者
/// </summary>
internal class CoreAccessProvider : IAccessProvider<CoreAccessSecret>
{
    public Task<CoreAccessSecret> Get()
    {
        var secret = new CoreAccessSecret();
        ConfigHelper.Configuration.GetSection("Client:Portal").Bind(secret);

        if (string.IsNullOrEmpty(secret.access_secret))
            throw new NoNullAllowedException("未能找到 Client:Portal 配置节点信息!");

        return Task.FromResult(secret);
    }
}