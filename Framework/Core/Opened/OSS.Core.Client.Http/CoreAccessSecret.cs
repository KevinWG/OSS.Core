using OSS.Common;

namespace OSS.Core.Client.Http;

/// <summary>
///  接口请求秘钥配置信息
/// </summary>
public class CoreAccessSecret : AccessSecret
{
    /// <summary>
    ///  接口请求域名地址
    /// </summary>
    public string api_domain { get; set; } = default!;
}