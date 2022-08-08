namespace OSS.Core.Module.Notify;

public enum NotifyChannel
{
    /// <summary>
    /// 系统平台
    /// </summary>
    SystemPlat = 1,

    /// <summary>
    ///  自身测试平台（不会实际发送
    /// </summary>
    SystemPlatTest = 2,

    /// <summary>
    ///  阿里云
    /// </summary>
    AliYun = 10,

    /// <summary>
    ///  华为云平台
    /// </summary>
    HwCloud = 20,
}