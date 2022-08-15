using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.WorkFlow;

/// <summary>
///  服务
/// </summary>
public class NodeMetaService : IOpenedNodeMetaService
{
    private static readonly NodeMetaRep _NodeMetaRep = new();


    /// <inheritdoc />
    public async Task<PageListResp<NodeMetaMo>> Search(SearchReq req)
    {
        return new PageListResp<NodeMetaMo>(await _NodeMetaRep.Search(req));
    }

    /// <inheritdoc />
    public Task<IResp<NodeMetaMo>> Get(long id) => _NodeMetaRep.GetById(id);


    /// <inheritdoc />
    public Task<IResp> SetUseable(long id, ushort flag)
    {
        return _NodeMetaRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<IResp> Add(AddNodeMetaReq req)
    {
        var mo = req.MapToNodeMetaMo();
        mo.FormatBaseByContext();

        await _NodeMetaRep.Add(mo);
        return Resp.DefaultSuccess;
    }
}
