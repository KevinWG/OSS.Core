namespace OSS.Core.Module.Pipeline;



/// <summary>
///     子流水线执行扩展信息
/// </summary>
public class SubPipeLineExt : BaseExecuteExt
{
    /// <summary>
    /// 对应的流水线Id
    /// </summary>
    public long pipe_id { get; set; }
}



/// <summary>
///     审核执行扩展信息
/// </summary>
public class AuditExt : BaseExecuteExt
{
    /// <summary>
    /// 对应的流水线Id
    /// </summary>
    public long pipe_id { get; set; }
}

