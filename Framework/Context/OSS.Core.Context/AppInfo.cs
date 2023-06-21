namespace OSS.Core.Context;

/// <summary>
///  应用信息
/// </summary>
public class AppInfo//:IAccessSecret
{
    /// <summary>
    ///  当前应用工作实例Id
    /// </summary>
    public int worker_id { get; set; }   

    /// <summary>
    /// 当前应用版本
    /// </summary>
    public string version { get; set; } = string.Empty;

    /// <summary>
    ///  应用基础访问地址
    /// </summary>
    public string base_url { get; set; } = string.Empty;
}