using OSS.Common.Resp;
using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  审核申请提交事件
/// </summary>
internal class StartActivity  : BasePassiveActivity<StartReq,IResp,FlowNodeMo>
{
    protected override Task<TrafficSignal<IResp, FlowNodeMo>> Executing(StartReq para)
    {
        throw new NotImplementedException();
    }
}