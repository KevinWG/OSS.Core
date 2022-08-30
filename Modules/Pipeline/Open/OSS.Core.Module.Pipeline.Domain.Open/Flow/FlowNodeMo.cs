
#region Copyright (C)  Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 实体对象
*
*　　	创建人： osscore
*    	创建日期：
*       
*****************************************************************************/

#endregion

using OSS.Core.Domain;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///   业务流 节点 对象实体 
/// </summary>
public class FlowNodeMo : BaseOwnerMo<long>, IDomainStatus<ProcessStatus>
{
    /// <summary>
    ///  所属管道Id
    /// </summary>
    public long pipe_id { get; set; }

    /// <summary>
    /// 管道类型
    /// </summary>
    public PipeType pipe_type { get; set; }


    #region 节点信息

    /// <summary>
    ///   父级业务流节点Id
    /// </summary>
    public long parent_id { get; set; }

    /// <summary>
    /// 业务 流程/节点  对应 业务Id
    /// </summary>
    public string biz_id { get; set; } = default!;

    /// <summary>
    /// 业务流程/节点名称
    /// </summary>
    public string title { get; set; } = default!;

    #endregion


    #region 处理进度

    /// <summary>
    /// 处理进度状态
    /// </summary>
    public ProcessStatus status { get; set; }

    /// <summary>
    ///   处理备注
    /// </summary>
    public string? remark { get; set; }
    
    /// <summary>
    ///  处理时间
    /// </summary>
    public long process_time { get; set; }

    /// <summary>
    ///  处理人Id
    /// </summary>
    public long processor_id { get; set; }

    /// <summary>
    /// 处理人名称
    /// </summary>
    public string? processor_name { get; set; }

    #endregion


}

