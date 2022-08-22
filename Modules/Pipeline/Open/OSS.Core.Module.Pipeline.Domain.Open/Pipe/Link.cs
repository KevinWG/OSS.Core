namespace OSS.Core.Module.Pipeline;

/// <summary>
///  链接信息
/// </summary>
public class Link
{
    /// <summary>
    /// 链接出发端 pipe id
    /// </summary>
    public long from { get; set; }

    /// <summary>
    /// 链接到达端 pipe Id
    /// </summary>
    public long to { get; set; }
}