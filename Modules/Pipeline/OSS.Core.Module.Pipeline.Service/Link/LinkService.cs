using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Link 服务
/// </summary>
public class LinkService : ILinkOpenService
{
    private static readonly LinkRep _LinkRep = new();


    /// <inheritdoc />
    public async Task<PageListResp<LinkMo>> Search(SearchReq req)
    {
        return new PageListResp<LinkMo>(await _LinkRep.Search(req));
    }

    /// <inheritdoc />
    public Task<IResp<LinkMo>> Get(long id) => _LinkRep.GetById(id);


    /// <inheritdoc />
    public Task<IResp> SetUseable(long id, ushort flag)
    {
        return _LinkRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<IResp> Add(AddLinkReq req)
    {
        var mo = req.MapToLinkMo();
        mo.FormatBaseByContext();

        await _LinkRep.Add(mo);
        return Resp.DefaultSuccess;
    }
}
