using OSS.Common.Resp;
using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  处理当前节点
/// </summary>
internal class FeedActivity : BasePassiveActivity<FeedReq,IResp,FlowNodeMo>
{
    protected override Task<TrafficSignal<IResp, FlowNodeMo>> Executing(FeedReq para)
    {
        throw new NotImplementedException();
    }
}