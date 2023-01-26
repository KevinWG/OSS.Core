using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;
using OSS.Common.Extension;

namespace TM.WMS;

/// <summary>
///  批次号 服务
/// </summary>
public class BatchService : IBatchOpenService
{
    private static readonly BatchRep _batchRep = new();

    /// <inheritdoc />
    public async Task<TokenPageListResp<BatchMo>> MSearch(SearchReq req)
    {
        var pageList = await _batchRep.Search(req);
         return pageList.ToTokenPageRespWithIdToken();
    }
    /// <inheritdoc />

    public async Task<Resp<BatchMo>> Get(long id)
    {
        var  getRes = await _batchRep.GetById(id);
        return getRes.IsSuccess() ? getRes : new Resp<BatchMo>().WithResp(getRes,"未能找到批次号信息");
    }

    ///// <inheritdoc />
    //public async Task<Resp<BatchCodeMo>> GetUseable(long id)
    //{
    //    var getRes = await Get(id);
    //    if (!getRes.IsSuccess())
    //        return getRes;
    //    
    //    return getRes.data.status >= 0 
    //        ? getRes
    //        : new Resp<BatchCodeMo>().WithResp(RespCodes.OperateObjectNull,"未能找到有效可用的批次号信息");
    //}

    /// <inheritdoc />
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
        var id = PassTokenHelper.GetData(pass_token).ToInt64();

        return _batchRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<LongResp> Add(AddBatchCodeReq req)
    {
        var maRes = await InsContainer<IMaterialService>.Instance.Get(req.material_id);
        if (!maRes.IsSuccess())
            return new LongResp().WithResp( maRes);

        var mo = req.ToMo(maRes.data);
        if (!string.IsNullOrEmpty(req.code))
        {
            var countRes = await _batchRep.GetCountByCode(req.code);
            if (countRes.data>0)
                return new LongResp().WithResp(RespCodes.OperateObjectExisted, "当前批次号编码已经存在!");
        }
        else
        {
            mo.code = string.Concat("PC", DateTime.Now.ToString("yyMMddHHmmss"));
        }

        await _batchRep.Add(mo);
        return new LongResp(mo.id);
    }
}
