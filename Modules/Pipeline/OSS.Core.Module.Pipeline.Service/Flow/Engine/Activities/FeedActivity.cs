using OSS.Common.Resp;
using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  处理当前节点
/// </summary>
internal class FeedActivity : BasePassiveActivity<FeedReq,IResp>
{
    protected override Task<TrafficSignal<IResp>> Executing(FeedReq para)
    {
        throw new NotImplementedException();
    }
}