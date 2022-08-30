using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Flow 服务
/// </summary>
public class FlowService : IFlowCommon
{
    private static readonly FlowNodeRep _fNodeRep = new();


    /// <inheritdoc />
    public async Task<PageListResp<FlowNodeMo>> Search(SearchReq req)
    {
        return new PageListResp<FlowNodeMo>(await _fNodeRep.Search(req));
    }

    /// <inheritdoc />
    public Task<IResp<FlowNodeMo>> Get(long id) => _fNodeRep.GetById(id);


    /// <inheritdoc />
    public Task<IResp> Start(StartReq req)
    {
        return FlowEngine.Start(req);
    }

    /// <inheritdoc />
    public Task<IResp> Feed(FeedReq req)
    {
        throw new NotImplementedException();
    }



    async Task<LongResp> IFlowCommon.AddNode(FlowNodeMo node)
    {
        await _fNodeRep.Add(node);
        return new LongResp(node.id);
    }

    /// <inheritdoc />
    public  Task<IResp> AddNodes(List<FlowNodeMo> nextNodes)
    {
        return _fNodeRep.AddList(nextNodes);
    }

    Task<IResp> IFlowCommon.UpdateNodeStatus(long id, ProcessStatus status)
    {
        return _fNodeRep.UpdateStatus(id, status);
    }
}