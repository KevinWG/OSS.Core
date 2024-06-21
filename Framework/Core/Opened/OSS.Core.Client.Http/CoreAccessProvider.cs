using Microsoft.Extensions.Configuration;
using OSS.Tools.Config;
using System.Data;
using OSS.Common.Resp;

namespace OSS.Core.Client.Http;

/// <summary>
///  接口请求秘钥提供者
/// </summary>
internal class CoreAccessProvider : ICoreAccessProvider
{
    public Task<CoreAccessSecret> Get(string moduleName)
    {
        if (ConfigHelper.Configuration == null)
        {
            throw new RespNotImplementException("请注册 ConfigHelper.Configuration 配置信息");
        }

        var secret = new CoreAccessSecret();
        ConfigHelper.Configuration.GetSection(string.Concat("RemoteService:", moduleName)).Bind(secret);

        if (!string.IsNullOrEmpty(secret.access_secret))
            return Task.FromResult(secret);

        ConfigHelper.Configuration.GetSection("RemoteService:Default").Bind(secret);
 
        if (string.IsNullOrEmpty(secret.access_secret))
            throw new NoNullAllowedException($"未能找到 RemoteService:{moduleName} 配置节点信息!");

        return Task.FromResult(secret);
    }
}