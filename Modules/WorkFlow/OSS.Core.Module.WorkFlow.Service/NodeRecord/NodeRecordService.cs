using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.WorkFlow;

/// <summary>
///  服务
/// </summary>
public class NodeRecordService : IOpenedNodeRecordService
{
    private static readonly NodeRecordRep _NodeRecordRep = new();


    /// <inheritdoc />
    public async Task<PageListResp<NodeRecordMo>> Search(SearchReq req)
    {
        return new PageListResp<NodeRecordMo>(await _NodeRecordRep.Search(req));
    }

    /// <inheritdoc />
    public Task<IResp<NodeRecordMo>> Get(long id) => _NodeRecordRep.GetById(id);


    /// <inheritdoc />
    public Task<IResp> SetUseable(long id, ushort flag)
    {
        return _NodeRecordRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<IResp> Add(AddNodeRecordReq req)
    {
        var mo = req.MapToNodeRecordMo();
        mo.FormatBaseByContext();

        await _NodeRecordRep.Add(mo);
        return Resp.DefaultSuccess;
    }
}
