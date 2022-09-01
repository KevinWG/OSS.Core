using OSS.Common;
using OSS.Common.Resp;
using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  审核申请提交事件
/// </summary>
internal class StartActivity : BasePassiveActivity<long,IResp,FlowNodeMo>
{

    protected override async Task<TrafficSignal<IResp, FlowNodeMo>> Executing(long flowId)
    {
        var startRes = await Start(flowId);

        return startRes.IsSuccess()
            ? new TrafficSignal<IResp, FlowNodeMo>(startRes, startRes.data)
            : new TrafficSignal<IResp, FlowNodeMo>(SignalFlag.Yellow_Wait, startRes);
    }


    private static async Task<IResp<FlowNodeMo>> Start(long flowId)
    {
        var flowNodeRes = await InsContainer<IFlowCommon>.Instance.Get(flowId);
        if (!flowNodeRes.IsSuccess())
            return flowNodeRes;

        var flow = flowNodeRes.data;

        var updateRes = await InsContainer<IFlowCommon>.Instance.UpdateStatus(flow, ProcessStatus.Processing);
        return updateRes.IsSuccess() ? new Resp<FlowNodeMo>(flow) : new Resp<FlowNodeMo>().WithResp(updateRes);
    }
}