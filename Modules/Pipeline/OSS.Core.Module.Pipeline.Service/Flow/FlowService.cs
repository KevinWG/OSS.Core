using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Flow 服务
/// </summary>
public class FlowService : IFlowCommonService
{
    private static readonly FlowNodeRep _FlowRep = new();


    /// <inheritdoc />
    public async Task<PageListResp<FlowNodeMo>> Search(SearchReq req)
    {
        return new PageListResp<FlowNodeMo>(await _FlowRep.Search(req));
    }

    /// <inheritdoc />
    public Task<IResp<FlowNodeMo>> Get(long id) => _FlowRep.GetById(id);


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

    async Task<LongResp> IFlowCommonService.AddNode(FlowNodeMo node)
    {
        await _FlowRep.Add(node);
        return new LongResp(node.id);
    }
}



/// <summary>
/// 业务流内部公用服务
/// </summary>
public interface IFlowCommonService: IFlowOpenService
{
    /// <summary>
    /// 添加流程节点
    /// </summary>
    /// <param name="flow"></param>
    /// <returns></returns>
    internal Task<LongResp> AddNode(FlowNodeMo flow);
}
