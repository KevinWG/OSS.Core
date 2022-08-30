using OSS.Common;
using OSS.Common.Resp;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Flow 对象仓储
/// </summary>
public class FlowNodeRep : BasePipelineRep<FlowNodeMo,long> 
{
    /// <inheritdoc />
    public FlowNodeRep() : base("Flow")
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<FlowNodeMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
    }

    /// <summary>
    ///   修改状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public Task<IResp> UpdateStatus(long id, ProcessStatus status)
    {
        return Update(u => new {u.status}, w => w.id == id, new {status});
    }
}
