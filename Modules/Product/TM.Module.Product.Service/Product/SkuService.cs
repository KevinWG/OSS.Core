using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace TM.Module.Product;

/// <summary>
///  Sku 服务
/// </summary>
public class SkuService : ISkuOpenService
{
    private static readonly SkuRep _SkuRep = new();


    /// <inheritdoc />
    public async Task<TokenPageListResp<SkuMo>> MSearch(SearchReq req)
    {
        var pageList = await _SkuRep.Search(req);
         return pageList.ToTokenPageRespWithIdToken();
    }

    /// <inheritdoc />
    public async Task<Resp<SkuMo>> Get(long id)
    {
        var  getRes = await _SkuRep.GetById(id);
        return getRes.IsSuccess() ? getRes : new Resp<SkuMo>().WithResp(getRes,"未能找到Sku信息");
    }

    /// <inheritdoc />
    public async Task<Resp<SkuMo>> GetUseable(long id)
    {
        var getRes = await Get(id);
        if (!getRes.IsSuccess())
            return getRes;
        
        return getRes.data.status >= 0 
            ? getRes
            : new Resp<SkuMo>().WithResp(RespCodes.OperateObjectNull,"未能找到有效可用的Sku信息");
    }

    /// <inheritdoc />
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
        var id = PassTokenHelper.GetData(pass_token).ToInt64();

        return _SkuRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<LongResp> Add(AddSkuReq req)
    {
        var mo = req.MapToSkuMo();

        await _SkuRep.Add(mo);
        return new LongResp(mo.id);
    }
}
