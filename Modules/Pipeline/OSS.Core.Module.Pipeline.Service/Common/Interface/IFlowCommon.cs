using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
/// 业务流内部公用服务
/// </summary>
internal interface IFlowCommon : IFlowOpenService
{
    /// <summary>
    /// 添加流程节点
    /// </summary>
    /// <param name="flow"></param>
    /// <returns></returns>
    internal Task<LongResp> AddNode(FlowNodeMo flow);

    /// <summary>
    /// 批量添加流程节点
    /// </summary>
    /// <param name="nextNodes"></param>
    /// <returns></returns>
    internal Task<IResp> AddNodes(List<FlowNodeMo> nextNodes);


    /// <summary>
    /// 修改节点处理人信息
    /// </summary>
    /// <param name="node"></param>
    /// <param name="processorId"></param>
    /// <param name="processorName"></param>
    /// <returns></returns>
    internal Task<IResp<FlowNodeMo>> UpdateProcessor(FlowNodeMo node, long processorId, string processorName);

    /// <summary>
    /// 修改节点 执行开始 状态
    /// </summary>
    /// <param name="node"></param>
    /// <param name="newStatus"></param>
    /// <param name="remark"></param>
    /// <returns></returns>
    internal Task<IResp<FlowNodeMo>> UpdateStatus(FlowNodeMo node,ProcessStatus newStatus, string remark="");

    /// <summary>
    ///   通过父级Id结束子节点
    /// </summary>
    /// <param name="parentId">父级流Id</param>
    /// <param name="endTime"></param>
    /// <param name="remark"></param>
    /// <returns></returns>
    internal Task<IResp> AbandonChildrenByParent(long parentId, long endTime, string remark);

}
