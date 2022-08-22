namespace OSS.Core.Module.Pipeline;

/// <summary>
/// 流水线信息
/// </summary>
public class Pipeline : PipeItem
{
    /// <summary>
    ///  当前版本名称
    /// </summary>
    public string? version { get; set; } 

    /// <summary>
    /// 流水线的定义id
    /// </summary>
    public long define_id { get; set; }
}