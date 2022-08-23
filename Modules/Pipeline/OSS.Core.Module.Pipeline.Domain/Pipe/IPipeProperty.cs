namespace OSS.Core.Module.Pipeline;

/// <summary>
/// 管道基础属性
/// </summary>
public interface IPipeProperty
{
    /// <summary>
    /// 管道类型
    /// </summary>
    public PipeType type { get; set; }

    /// <summary>
    ///  扩展信息
    /// </summary>
    public string? execute_ext { get; set; }

    /// <summary>
    ///  父级 Pipeline id
    /// </summary>
    public long parent_id { get; set; }
}