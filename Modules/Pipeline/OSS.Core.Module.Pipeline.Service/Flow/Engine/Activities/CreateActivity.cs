
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
         if (!addRes.IsSuccess())
             return new TrafficSignal<LongResp, CreateContext>(SignalFlag.Yellow_Wait, addRes);

         return new TrafficSignal<LongResp, CreateContext>(addRes, new CreateContext(addRes.data, req.manual > 0));
    }

    private static async Task<LongResp> AddFlow(CreateReq req)
    {
        var pipelineRes = await InsContainer<IPipelinePartCommon>.Instance.GetLine(req.pipeline_id);
        if (!pipelineRes.IsSuccess())
            return new LongResp().WithResp(pipelineRes);

        var pipeline = pipelineRes.data;
        var isManual = req.manual > 0;

        var flow = pipeline.ToFlowMo();

        flow.biz_id    = req.biz_id;
        flow.parent_id = req.parent_id;
        flow.status    = isManual ? ProcessStatus.Processing : ProcessStatus.Waiting;

        flow.FormatBaseByContext();

        return await InsContainer<IFlowCommonService>.Instance.AddNode(flow);
    }
}

/// <summary>
///   业务流创建后上下文
/// </summary>
/// <param name="flow_id"></param>
/// <param name="is_manual"></param>
public record struct CreateContext(long flow_id,bool is_manual);