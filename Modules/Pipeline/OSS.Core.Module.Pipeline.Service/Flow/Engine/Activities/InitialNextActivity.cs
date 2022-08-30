using OSS.Common;
using OSS.Common.Resp;
using OSS.Pipeline;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  初始化所有后续节点
/// </summary>
internal class InitialNextActivity : BaseEffectActivity<FlowNodeMo, IList<FlowNodeMo>>
{
    protected override async Task<TrafficSignal<IList<FlowNodeMo>>> Executing(FlowNodeMo node)
    {
        var iniRes = await Initial(node);
        return iniRes.IsSuccess()
            ? new TrafficSignal<IList<FlowNodeMo>>(iniRes.data)
            : new TrafficSignal<IList<FlowNodeMo>>(SignalFlag.Yellow_Wait, iniRes.data);
    }

    private static async Task<ListResp<FlowNodeMo>> Initial(FlowNodeMo node)
    {
        var pipeRes = await GetNextPipes(node.pipeline_id, node.pipe_id);
        if (!pipeRes.IsSuccess())
            return new ListResp<FlowNodeMo>().WithResp(pipeRes);

        var parentId  = node.pipe_type == Pipeline.PipeType.Pipeline ? node.id : node.parent_id;
        var nextNodes = pipeRes.data.Select(p => FormatNextMo(p, node, parentId)).ToList();

        var res = await InsContainer<IFlowCommon>.Instance.AddNodes(nextNodes);
        return res.IsSuccess() ? new ListResp<FlowNodeMo>(nextNodes) : new ListResp<FlowNodeMo>().WithResp(res);
    }

    private static async Task<ListResp<PipeView>> GetNextPipes(long pipelineId, long currentPipeId)
    {
        var pipelineRes = await InsContainer<IPipelineCommon>.Instance.GetDetail(pipelineId);
        if (!pipelineRes.IsSuccess())
            return new ListResp<PipeView>().WithResp(pipelineRes);
        
        var pipes = pipelineRes.data.items;

        if (currentPipeId<=0)
        {
            var items = pipes.Where(p => p.type == Pipeline.PipeType.Start).ToList();
            return items.Count > 0
                ? new ListResp<PipeView>(items)
                : new ListResp<PipeView>().WithResp(RespCodes.OperateObjectNull, "未能找到有效下级节点");
        }

        var links          = pipelineRes.data.links;
        var availableLinks = links.Where(l => l.from == currentPipeId).Select(l=>l.to).ToList();

       var targetPipes =  pipes.Where(p => availableLinks.Contains(p.id)).ToList();
       return targetPipes.Any()
           ? new ListResp<PipeView>(targetPipes)
           : new ListResp<PipeView>().WithResp(RespCodes.OperateObjectNull, "未能找到有效下级节点");
    }


    private static FlowNodeMo FormatNextMo(PipeView pipe, FlowNodeMo currentNode, long parentNodeId)
    {
        var node = new FlowNodeMo
        {
            pipe_id   = pipe.id,
            pipe_type = pipe.type,

            pipeline_id = pipe.parent_id,

            title  = pipe.name,
            status = ProcessStatus.Waiting,
            biz_id = currentNode.biz_id,

            parent_id = parentNodeId
        };

        return node;
    }

}