namespace OSS.Core.Module.Pipeline;

/// <summary>
///   版本状态
/// </summary>
public enum PipelineStatus
{
    /// <summary>
    ///   初始创建
    /// </summary>
    Original =0,

    /// <summary>
    /// 已发布
    /// </summary>
    Published = 1000,

    /// <summary>
    ///  下线
    /// </summary>
    OffLine = -100,

    /// <summary>
    ///  删除
    /// </summary>
    Deleted = -10000
}