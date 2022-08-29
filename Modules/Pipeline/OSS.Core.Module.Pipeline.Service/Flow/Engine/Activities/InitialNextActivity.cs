namespace OSS.Core.Module.Pipeline;

/// <summary>
///  初始化所有后续节点
/// </summary>
internal class InitialNextActivity
{
}

/// <summary>
///  初始化下个节点请求
/// </summary>
internal class InitialNextReq
{
    /// <summary>
    ///  流程id
    /// </summary>
    public long flow_id { get; set; }

    /// <summary>
    ///  节点id
    /// </summary>
    public long node_id { get; set; }
}
