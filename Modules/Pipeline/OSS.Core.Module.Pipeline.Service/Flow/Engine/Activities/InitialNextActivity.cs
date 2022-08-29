using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  初始化所有后续节点
/// </summary>
internal class InitialNextActivity:BaseActivity<InitialNextReq>
{
    protected override Task<TrafficSignal> Executing(InitialNextReq para)
    {
        return Task.FromResult(TrafficSignal.GreenSignal);
    }
}

/// <summary>
///  初始化下个节点请求
/// </summary>
internal record struct InitialNextReq(long flow_id, long node_id);