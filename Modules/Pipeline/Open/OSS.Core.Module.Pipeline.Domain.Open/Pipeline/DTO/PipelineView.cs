namespace OSS.Core.Module.Pipeline;

/// <summary>
/// 流水线信息
/// </summary>
public class PipelineView : PipeView
{

    /// <summary>
    ///   版本名称
    /// </summary>
    public string ver_name { get; set; } = string.Empty;

    /// <summary>
    /// 流水线的定义id
    /// </summary>
    public long meta_id { get; set; }

    /// <summary>
    ///  当前版本的链接信息
    /// </summary>
    public List<Link> links { get; set; } = default!;
    
    /// <summary>
    /// 版本状态
    /// </summary>
    public PipelineStatus status { get; set; }
}