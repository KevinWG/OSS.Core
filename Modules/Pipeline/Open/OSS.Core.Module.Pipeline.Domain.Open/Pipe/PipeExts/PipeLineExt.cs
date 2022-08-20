namespace OSS.Core.Module.Pipeline;

/// <summary>
/// 流水线扩展信息
/// </summary>
public class PipeLineExt: BasePipeExt
{
    /// <summary>
    /// 管道 连接关系
    /// </summary>
    public List<PipeLinkItem> links { get; set; } 
}


public class SubPipeLineExt : BasePipeExt
{
    /// <summary>
    /// 对应的流水线Id
    /// </summary>
    public long pipeline_id { get; set; }
}