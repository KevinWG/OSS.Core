using OSS.Common;
using OSS.Common.Resp;
using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

internal class ExecutingActivity : BaseActivity<FlowNodeMo>
{
    protected override async Task<TrafficSignal> Executing(FlowNodeMo para)
    {
        var eRes = await ExecutePipe(para);

        return eRes.IsSuccess()
            ? TrafficSignal.GreenSignal
            : new TrafficSignal(SignalFlag.Yellow_Wait,string.Empty);
    }

    private static async Task<IResp> ExecutePipe(FlowNodeMo node)
    {
        return await InsContainer<IFlowCommon>.Instance.UpdateProcessor(node,1,"系统测试");
    }
}