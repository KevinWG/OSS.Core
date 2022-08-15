namespace OSS.Core.Module.WorkFlow;

public enum NodeType
{
    /// <summary>
    /// 开始节点
    /// </summary>
    FirstNode = 0,

    /// <summary>
    /// 正常处理节点
    /// </summary>
    NormalNode = 100,

    /// <summary>
    /// 最后节点
    /// </summary>
    FinalNode = 10000
}