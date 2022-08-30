namespace OSS.Core.Module.Pipeline;

/// <summary>
///  创建业务流
/// </summary>
public class CreateReq : BaseExecutingReq
{
    /// <summary>
    ///  所属管道Id
    /// </summary>
    public long pipeline_id { get; set; }

    /// <summary>
    /// 业务 流程/节点  对应 业务Id
    /// </summary>
    public string biz_id { get; set; } = default!;

    /// <summary>
    ///   是否手动启动 （ 0 - 创建后开始运行，1 - 创建后手动开始运行 ）
    /// </summary>
    public int manual { get; set; }

    /// <summary>
    ///  父级流程Id
    /// </summary>
    public long parent_id { get; set; }
}