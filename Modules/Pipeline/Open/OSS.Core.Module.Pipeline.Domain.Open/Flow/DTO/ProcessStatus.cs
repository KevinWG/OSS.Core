using OSS.Common.Extension;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  进度状态
/// </summary>
public enum ProcessStatus
{
    /// <summary>
    /// 等待处理
    /// </summary>
    [OSDescribe("等待处理")]
    Waiting = 0,

    /// <summary>
    ///  进行中
    /// </summary>
    [OSDescribe("进行中")] 
    Processing = 10,

    ///// <summary>
    /////  暂停挂起
    ///// </summary>
    //Suspend = 100,

    /// <summary>
    ///   中止
    /// </summary>
    [OSDescribe("废弃中止")] 
    Abandon = -1000,

    /// <summary>
    ///  完成
    /// </summary>
    [OSDescribe("完成")]
    Completed = 10000,
}