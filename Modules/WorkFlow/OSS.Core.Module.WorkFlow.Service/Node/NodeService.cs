using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.WorkFlow;

/// <summary>
///  服务
/// </summary>
public class NodeService : IOpenedNodeService
{
    private static readonly NodeRep _NodeRep = new();


    /// <inheritdoc />
    public async Task<PageListResp<NodeMo>> Search(SearchReq req)
    {
        return new PageListResp<NodeMo>(await _NodeRep.Search(req));
    }

    /// <inheritdoc />
    public Task<IResp<NodeMo>> Get(long id) => _NodeRep.GetById(id);


    /// <inheritdoc />
    public Task<IResp> SetUseable(long id, ushort flag)
    {
        return _NodeRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<IResp> Add(AddNodeReq req)
    {
        var mo = req.MapToNodeMo();
        mo.FormatBaseByContext();

        await _NodeRep.Add(mo);
        return Resp.DefaultSuccess;
    }
}
