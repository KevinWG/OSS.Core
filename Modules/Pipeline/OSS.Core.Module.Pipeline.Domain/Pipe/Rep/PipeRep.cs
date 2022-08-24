using System.Text;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Pipe 对象仓储
/// </summary>
public class PipeRep : BasePipelineRep<PipeMo,long> 
{
    /// <inheritdoc />
    public PipeRep() : base(PipelineConst.RepTables.Pipe)
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<PipeMo>> Search(SearchReq req)
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

    /// <summary>
    ///  获取所有子节点
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public Task<List<PipeMo>> GetListByParentId(long parentId)
    {
        return GetList(p => p.parent_id == parentId);
    }
}
