using OSS.Common;
using OSS.Common.Resp;
using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  审核申请提交事件
/// </summary>
internal class StartActivity  : BasePassiveActivity<StartReq,IResp,FlowNodeMo>
{
    protected override async Task<TrafficSignal<IResp, FlowNodeMo>> Executing(StartReq para)
    {
        var startRes = await StartFlow(para.flow_id);

        return startRes.IsSuccess()
            ? new TrafficSignal<IResp, FlowNodeMo>(startRes, startRes.data)
            : new TrafficSignal<IResp, FlowNodeMo>(SignalFlag.Yellow_Wait, startRes);
    }

    private static async Task<IResp<FlowNodeMo>> StartFlow(long flowId)
    {
        var flowService = InsContainer<IFlowCommon>.Instance;

        var flowNodeRes = await flowService.Get(flowId);
        if (!flowNodeRes.IsSuccess())
            return flowNodeRes;

        var flow = flowNodeRes.data;
        if (flow.status != ProcessStatus.Waiting)
            return new Resp<FlowNodeMo>().WithResp(RespCodes.OperateFailed, "当前流程已经启动，无法重新启动!");

        flow.status = ProcessStatus.Processing;

        var updateRes = await flowService.UpdateNodeStatus(flowId, flow.status);
        return updateRes.IsSuccess() ? new Resp<FlowNodeMo>(flow) : new Resp<FlowNodeMo>().WithResp(updateRes);
    }
}