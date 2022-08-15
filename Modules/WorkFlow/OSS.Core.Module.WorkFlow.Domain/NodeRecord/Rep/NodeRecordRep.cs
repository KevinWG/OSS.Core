using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.WorkFlow;

public class NodeRecordRep : BaseWorkFlowRep<NodeRecordMo,long> 
{
    public NodeRecordRep() : base("NodeRecord")
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<NodeRecordMo>> Search(SearchReq req)
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
