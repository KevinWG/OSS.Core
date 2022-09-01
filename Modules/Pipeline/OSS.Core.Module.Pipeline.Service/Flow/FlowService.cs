using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Flow 服务
/// </summary>
public class FlowService : IFlowCommon
{
    private static readonly FlowNodeRep _fNodeRep = new();

    /// <inheritdoc />
    public async Task<PageListResp<FlowNodeMo>> Search(SearchReq req)
    {
        return new PageListResp<FlowNodeMo>(await _fNodeRep.Search(req));
    }

    /// <inheritdoc />
    public Task<IResp<FlowNodeMo>> Get(long id) => _fNodeRep.GetById(id);

    /// <inheritdoc />
    public  Task<IResp> Start(long flowId)
    {
        return FlowEngine.Start(flowId);
    }

    /// <inheritdoc />
    public Task<IResp> Feed(FeedReq req)
    {
        return FlowEngine.Feed(req);
    }



    // =========================   内部公用    ========================= 



    /// <inheritdoc />
    Task<IResp> IFlowCommon.AddNodes(List<FlowNodeMo> nextNodes)
    {
        return _fNodeRep.AddList(nextNodes);
    }

    /// <inheritdoc />
    async Task<LongResp> IFlowCommon.AddNode(FlowNodeMo node)
    {
        await _fNodeRep.Add(node);
        return new LongResp(node.id);
    }

    async Task<IResp<FlowNodeMo>> IFlowCommon.UpdateProcessor(FlowNodeMo node, long processorId, string processorName)
    {
        node.processor_id   = processorId;
        node.processor_name = processorName;

        var updateRes = await _fNodeRep.UpdateProcessor(node.id, processorId, processorName);
        return updateRes.IsSuccess() ? new Resp<FlowNodeMo>(node) : new Resp<FlowNodeMo>().WithResp(updateRes);
    }

    async Task<IResp<FlowNodeMo>> IFlowCommon.UpdateStatus(FlowNodeMo node, ProcessStatus newStatus, string remark)
    {
        if (Math.Abs((int) newStatus) <= Math.Abs((int) node.status))
            return new Resp<FlowNodeMo>().WithResp(RespCodes.OperateFailed, $"当前节点状态({node.status.GetDesp()}),修改状态失败!");

        node.status = newStatus;
        node.remark = remark;

        var isStart = newStatus == ProcessStatus.Processing;
        var time    = DateTime.Now.ToUtcSeconds();

        var updateRes = await (isStart
            ? _fNodeRep.UpdateStart(node.id, time)
            : _fNodeRep.UpdateEnd(node.id, newStatus, time, remark));

        return updateRes.IsSuccess()
            ? new Resp<FlowNodeMo>(node)
            : new Resp<FlowNodeMo>().WithResp(RespCodes.OperateFailed, "修改节点状态失败!");
    }

    Task<IResp> IFlowCommon.AbandonChildrenByParent(long parentId, long endTime, string remark)
    {
        return _fNodeRep.AbandonChildrenByParent(parentId, endTime, remark);
    }
}