using OSS.Common;
using OSS.Common.Resp;
using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  处理当前节点
/// </summary>
internal class FeedActivity : BasePassiveActivity<FeedReq, IResp, FlowNodeMo>
{
    protected override async Task<TrafficSignal<IResp, FlowNodeMo>> Executing(FeedReq para)
    {
        var feedRes = await Feed(para);

        return feedRes.IsSuccess()
            ? new TrafficSignal<IResp, FlowNodeMo>(feedRes, feedRes.data)
            : new TrafficSignal<IResp, FlowNodeMo>(SignalFlag.Yellow_Wait, feedRes);
    }

    private static async Task<IResp<FlowNodeMo>> Feed(FeedReq req)
    {
        var nodeRes = await InsContainer<IFlowCommon>.Instance.Get(req.node_id);
        if (!nodeRes.IsSuccess())
            return nodeRes;

        var node = nodeRes.data;
        
        return await InsContainer<IFlowCommon>.Instance.UpdateStatus(node, req.status,req.remark);
    }
}