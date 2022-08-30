using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
/// 业务流内部公用服务
/// </summary>
public interface IFlowCommon: IFlowOpenService
{


    /// <summary>
    /// 修改节点状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    internal Task<IResp> UpdateNodeStatus(long id, ProcessStatus status);

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
    Task<IResp> AddNodes(List<FlowNodeMo> nextNodes);
}