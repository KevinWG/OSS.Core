using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;
using OSS.Common.Extension;

namespace OSS.Core.Module.Portal;

/// <summary>
///  组织机构 服务
/// </summary>
public class OrganizationService : IOrganizationOpenService
{
    private static readonly IOrganizationRep _orgRep = InsContainer<IOrganizationRep>.Instance;

    /// <inheritdoc />
    public async Task<TokenPageListResp<OrganizationMo>> MSearch(SearchReq req)
    {
        var pageList = await _orgRep.Search(req);
         return pageList.ToTokenPageRespWithIdToken();
    }

    /// <inheritdoc />
    public async Task<PageListResp<OrganizationMo>> ComSearch(SearchReq req)
    {
        var pageList = await _orgRep.Search(req);
        return new PageListResp<OrganizationMo>(pageList);
    }

    /// <inheritdoc />
    public async Task<Resp<OrganizationMo>> Get(long id)
    {
        var  getRes = await _orgRep.GetById(id);
        return getRes.IsSuccess() ? getRes : new Resp<OrganizationMo>().WithResp(getRes,"未能找到组织机构信息");
    }

    /// <inheritdoc />
    public async Task<Resp<OrganizationMo>> GetUseable(long id)
    {
        var getRes = await Get(id);
        if (!getRes.IsSuccess())
            return getRes;
        
        return getRes.data.status >= 0 
            ? getRes
            : new Resp<OrganizationMo>().WithResp(RespCodes.OperateObjectNull,"未能找到有效可用的组织机构信息");
    }

    /// <inheritdoc />
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
        var id = PassTokenHelper.GetData(pass_token).ToInt64();

        return _orgRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<LongResp> Add(AddOrganizationReq req)
    {
        var mo = req.MapToOrganizationMo();

        await _orgRep.Add(mo);
        return new LongResp(mo.id);
    }
}
