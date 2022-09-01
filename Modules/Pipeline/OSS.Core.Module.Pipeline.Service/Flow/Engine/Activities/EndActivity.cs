using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  结束节点
/// </summary>
internal class EndActivity:BaseActivity<FlowNodeMo>
{
    protected override async Task<TrafficSignal> Executing(FlowNodeMo node)
    {
        var endRes = await EndFlow(node);
        return endRes.IsSuccess()
            ? TrafficSignal.GreenSignal
            : new TrafficSignal(SignalFlag.Yellow_Wait, string.Empty);
    }

    private static async Task<IResp> EndFlow(FlowNodeMo node)
    {
        var parentNodeRes = await InsContainer<IFlowCommon>.Instance.Get(node.parent_id);
        if (!parentNodeRes.IsSuccess())
            return parentNodeRes;

        var flow = parentNodeRes.data;
        if (node.status == ProcessStatus.Abandon)
        {
          return await AbandonFlow(flow, node);
        }

        return await InsContainer<IFlowCommon>.Instance.UpdateStatus(flow, ProcessStatus.Completed,
            $"{node.remark}(执行人员:{node.processor_name})");
    }
    
    private static async Task<IResp> AbandonFlow(FlowNodeMo flow, FlowNodeMo node)
    {
        var dateTime    = DateTime.Now.ToUtcSeconds();

        var flowService = InsContainer<IFlowCommon>.Instance;

        await flowService.AbandonChildrenByParent(flow.id, dateTime, "主流程中止!");

        return await flowService.UpdateStatus(flow, ProcessStatus.Abandon,
            $"{node.remark}(执行人员:{node.processor_name})");
    }
}