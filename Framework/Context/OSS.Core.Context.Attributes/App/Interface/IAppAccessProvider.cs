using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Context.Attributes;

/// <summary>
///  签名秘钥提供者
/// </summary>
public interface IAppAccessProvider
{
    /// <summary>
    ///  通过Key值获取应用签名信息
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task<Resp<AppAccess>> GetByKey(string key);
}

/// <summary>
/// 签名秘钥信息
/// </summary>
public class AppAccess : AccessSecret
{
    /// <summary>
    ///  应用类型
    /// </summary>
    public AppType type { get; set; } = AppType.Normal;

    /// <summary>
    ///  签名时间戳有效时长(秒数,默认五分钟)
    ///     客户端根据时间戳以及其他参数生成签名，服务端校验签名，并比对服务端的时间
    /// </summary>
    public int sign_expire_time { get; set; } = 5 * 60;
}