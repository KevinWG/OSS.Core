using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace TM.Module.Product;

/// <summary>
///  产品 服务
/// </summary>
public class SpuService : ISpuOpenService
{
    private static readonly SpuRep _ProductRep = new();


    /// <inheritdoc />
    public async Task<TokenPageListResp<SpuMo>> MSearch(SearchReq req)
    {
        var pageList = await _ProductRep.Search(req);
         return pageList.ToTokenPageRespWithIdToken();
    }

    /// <inheritdoc />
    public async Task<Resp<SpuMo>> Get(long id)
    {
        var  getRes = await _ProductRep.GetById(id);
        return getRes.IsSuccess() ? getRes : new Resp<SpuMo>().WithResp(getRes,"未能找到产品信息");
    }

    /// <inheritdoc />
    public async Task<Resp<SpuMo>> GetUseable(long id)
    {
        var getRes = await Get(id);
        if (!getRes.IsSuccess())
            return getRes;
        
        return getRes.data.status >= 0 
            ? getRes
            : new Resp<SpuMo>().WithResp(RespCodes.OperateObjectNull,"未能找到有效可用的产品信息");
    }

    /// <inheritdoc />
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
        var id = PassTokenHelper.GetData(pass_token).ToInt64();

        return _ProductRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<LongResp> Add(AddSpuReq req)
    {
        var mo = req.MapToProductMo();

        await _ProductRep.Add(mo);
        return new LongResp(mo.id);
    }
}
