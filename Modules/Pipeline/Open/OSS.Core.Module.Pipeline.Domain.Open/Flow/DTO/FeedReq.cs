namespace OSS.Core.Module.Pipeline;

/// <summary>
///  节点执行请求
/// </summary>
public class FeedReq : BaseExecutingReq
{
    /// <summary>
    ///  业务流节点Id
    /// </summary>
    public long node_id { get; set; }

    /// <summary>
    ///  处理状态
    /// </summary>
    public ProcessStatus status { get; set; }
}


/// <summary>
///  基础执行请求
/// </summary>
public class BaseExecutingReq
{
    // todo 扩展表单关联
}