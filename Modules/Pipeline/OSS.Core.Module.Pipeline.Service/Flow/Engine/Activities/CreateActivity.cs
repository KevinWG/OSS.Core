
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;
using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///   创建业务流程
/// </summary>
internal class CreateActivity : BasePassiveActivity<CreateReq, LongResp, CreateContext>
{
    /// <inheritdoc />
    public CreateActivity() : base(nameof(CreateActivity))
    {
    }

    protected override async Task<TrafficSignal<LongResp, CreateContext>> Executing(CreateReq req)
    {
         var addRes= await AddFlow(req);
         return !addRes.IsSuccess() 
             ? new TrafficSignal<LongResp, CreateContext>(SignalFlag.Yellow_Wait, new LongResp().WithResp(addRes)) 
             : new TrafficSignal<LongResp, CreateContext>(new LongResp(addRes.data.id), new CreateContext(addRes.data, req.manual > 0));
    }

    private static async Task<IResp<FlowNodeMo>> AddFlow(CreateReq req)
    {
        var pipelineRes = await InsContainer<IPipelinePartCommon>.Instance.GetLine(req.pipeline_id);
        if (!pipelineRes.IsSuccess())
            return new Resp<FlowNodeMo>().WithResp(pipelineRes);

        var flow = pipelineRes.data.ToFlowMo();

        flow.biz_id    = req.biz_id;
        flow.parent_id = req.parent_id;
        flow.status    = req.manual > 0 ? ProcessStatus.Processing : ProcessStatus.Waiting;

        flow.FormatBaseByContext();

        await InsContainer<IFlowCommonService>.Instance.AddNode(flow);
        return new Resp<FlowNodeMo>(flow);
    }
}

/// <summary>
///   业务流创建后上下文
/// </summary>
/// <param name="flow"></param>
/// <param name="is_manual"></param>
public record struct CreateContext(FlowNodeMo flow,bool is_manual);