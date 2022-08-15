namespace OSS.Core.Module.WorkFlow;

/// <summary>
///  节点实例类型
/// </summary>
public enum NodeInsType
{
    /// <summary>
    ///  管理员审核节点
    /// </summary>
    Manual_Admin = 10,

    /// <summary>
    ///  用户确认节点
    /// </summary>
    Manual_User = 20,

    /// <summary>
    ///  （特例）系统退款执行节点
    /// </summary>
    OrderRefund = 1100,
}