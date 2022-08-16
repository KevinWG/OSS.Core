﻿using OSS.Common;

namespace OSS.Core.Client.Http;

public static class CoreClientHelper
{
    /// <summary>
    ///   应用服务端签名模式，对应的票据信息的请求头名称
    /// </summary>
    public static string HeaderName { get; set; } = "at-id";

    /// <summary>
    ///  客户端接口请求访问秘钥
    /// </summary>
    public static ICoreAccessProvider AccessProvider { get; set; } = SingleInstance<CoreAccessProvider>.Instance;
}


/// <summary>
/// 访问信息提供者
/// </summary>
public interface ICoreAccessProvider
{
    /// <summary>
    /// 获取访问配置信息
    /// </summary>
    /// <returns></returns>
    Task<CoreAccessSecret> Get(string moduleName);
}