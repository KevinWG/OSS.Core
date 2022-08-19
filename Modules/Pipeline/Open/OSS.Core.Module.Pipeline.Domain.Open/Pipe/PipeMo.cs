
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
///  管道对象实体 
/// </summary>
public class PipeMo : BaseOwnerAndStateMo<long>
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
    public string? ext { get; set; }

    /// <summary>
    ///  父级id
    /// </summary>
    public long parent_id { get; set; }

}