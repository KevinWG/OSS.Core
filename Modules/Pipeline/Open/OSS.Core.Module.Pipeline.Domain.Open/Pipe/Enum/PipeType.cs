namespace OSS.Core.Module.Pipeline;

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
    ///  流水线
    /// </summary>
    Pipeline =10,

    /// <summary>
    ///  
    /// </summary>
    SubPipeline =20,



    /// <summary>
    ///  管理员审核节点
    /// </summary>
    AdminAudit = 1100,

    /// <summary>
    ///  用户确认节点
    /// </summary>
    UserConfirm = 1110,


    /// <summary>
    /// 自定义
    /// </summary>
    Custom = 10000,

    

    /// <summary>
    /// 结束节点
    /// </summary>
    End = 10000000,
}