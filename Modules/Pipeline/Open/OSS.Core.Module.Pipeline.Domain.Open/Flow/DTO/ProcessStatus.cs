namespace OSS.Core.Module.Pipeline;

/// <summary>
///  进度状态
/// </summary>
public enum ProcessStatus
{
    /// <summary>
    /// 等待处理
    /// </summary>
    Waiting = 0,

    /// <summary>
    ///  进行中
    /// </summary>
    Processing = 10,

    ///// <summary>
    /////  暂停挂起
    ///// </summary>
    //Suspend = 100,

    /// <summary>
    ///   中止
    /// </summary>
    Abandon = -1000,

    /// <summary>
    ///  完成
    /// </summary>
    Completed = 10000,
}