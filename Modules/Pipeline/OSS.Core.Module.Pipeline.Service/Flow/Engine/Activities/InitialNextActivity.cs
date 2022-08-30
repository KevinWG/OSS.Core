using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  初始化所有后续节点
/// </summary>
internal class InitialNextActivity: BaseEffectActivity<FlowNodeMo,IList<FlowNodeMo>>
{
    protected override Task<TrafficSignal<IList<FlowNodeMo>>> Executing(FlowNodeMo para)
    {
        throw new NotImplementedException();
    }
}