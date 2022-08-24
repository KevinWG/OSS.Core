namespace OSS.Core.Module.Pipeline;

/// <summary>
///  流水线和节点列表信息
/// </summary>
public class PipelineDetailView
{
    /// <summary>
    /// 流水线信息
    /// </summary>
    public PipelineView pipeline { get; set; } = default!;

    /// <summary>
    ///  当前版本的链接信息
    /// </summary>
    public List<Link> links { get; set; } = default!;

    /// <summary>
    ///     流水线管道节点列表
    /// </summary>
    public List<PipeView> items { get; set; }
}