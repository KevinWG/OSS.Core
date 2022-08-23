﻿using OSS.Core.Domain;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  管道对象实体 
/// </summary>
public class PipeMo : BaseOwnerAndStateMo<long>, IPipeProperty
{
    /// <summary>
    /// 管道节点名称
    /// </summary>
    public string name { get; set; } = default!;

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

