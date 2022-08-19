
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
///  Pipe 对象实体 
/// </summary>
public class PipeMo : BaseOwnerAndStateMo<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; } = default!;

    /// <summary>
    /// 管道类型
    /// </summary>
    public PipeType pipe_type { get; set; }

    /// <summary>
    ///  父级id
    /// </summary>
    public long parent_id { get; set; }
}

/// <summary>
///  管道类型
/// </summary>
public enum PipeType
{
    /// <summary>
    /// 开始节点
    /// </summary>
    Start = 0,

    /// <summary>
    /// 结束节点
    /// </summary>
    End = 1000000,



    /// <summary>
    /// 自定义
    /// </summary>
    Custom = 100000,




    /// <summary>
    ///  管理员审核节点
    /// </summary>
    AdminAudit = 100,

    /// <summary>
    ///  用户确认节点
    /// </summary>
    UserConfirm = 110,
}
