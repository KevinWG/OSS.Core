namespace OSS.Core.Module.Portal;

public enum QRProgress
{
    /// <summary>
    ///  待扫码
    /// </summary>
    WaitScan = 0,

    /// <summary>
    ///  业务执行处理中
    /// </summary>
    Processing = 100,
        
    /// <summary>
    /// 处理结束
    /// </summary>
    ProcessEnd = 1000
}