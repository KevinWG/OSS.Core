using OSS.Common.Resp;
using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  审核申请提交事件
/// </summary>
internal class StartActivity  : BasePassiveActivity<StartReq,IResp,InitialNextReq>
{
    protected override Task<TrafficSignal<IResp, InitialNextReq>> Executing(StartReq para)
    {
        throw new NotImplementedException();
    }
}