using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

internal class ExecutingActivity : BaseActivity<FlowNodeMo>
{
    protected override Task<TrafficSignal> Executing(FlowNodeMo para)
    {
        return  Task.FromResult(TrafficSignal.GreenSignal);
    }
}
