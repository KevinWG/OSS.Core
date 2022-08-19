using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Flow 对象仓储
/// </summary>
public class FlowRep : BasePipelineRep<FlowMo,long> 
{
    /// <inheritdoc />
    public FlowRep() : base("Flow")
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<FlowMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
    }

    /// <summary>
    ///   修改状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public Task<IResp> UpdateStatus(long id, CommonStatus status)
    {
        return Update(u => new {u.status}, w => w.id == id, new {status});
    }
}
