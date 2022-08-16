using OSS.Common;

namespace OSS.Core.Client.Http;

public static class CoreClientHelper
{
    /// <summary>
    ///   应用服务端签名模式，对应的票据信息的请求头名称
    /// </summary>
    public static string HeaderName { get; set; } = "at-id";

    public static IAccessProvider<CoreAccessSecret> AccessProvider { get; set; } = SingleInstance<CoreAccessProvider>.Instance;
}