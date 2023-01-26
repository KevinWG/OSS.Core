using OSS.Common.Resp;

namespace TM.WMS;

/// <summary>
///  单位 服务
/// </summary>
public class UnitService : IUnitOpenService
{
    private static readonly UnitRep _unitRep = new();

    /// <inheritdoc />
    public async Task<ListResp<UnitView>> All()
    {
        var list =await _unitRep.GetAll();
        return new ListResp<UnitView>(list.Select(u=>u.ToView()).ToList());
    }

    ///// <inheritdoc />
    //public async Task<IResp<UnitMo>> Get(long id)
    //{
    //    var  getRes = await _UnitRep.GetById(id);
    //    return getRes.IsSuccess() ? getRes : new Resp<UnitMo>().WithResp(getRes,"未能找到单位信息");
    //}

    ///// <inheritdoc />
    //public async Task<IResp<UnitMo>> GetUseable(long id)
    //{
    //    var getRes = await Get(id);
    //    if (!getRes.IsSuccess())
    //        return getRes;
        
    //    return getRes.data.status >= 0 
    //        ? getRes
    //        : new Resp<UnitMo>().WithResp(RespCodes.OperateObjectNull,"未能找到有效可用的单位信息");
    //}

    ///// <inheritdoc />
    //public Task<IResp> SetUseable(string pass_token, ushort flag)
    //{
    //    var id = PassTokenHelper.GetData(pass_token).ToInt64();

    //    return _UnitRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    //}
    
    /// <inheritdoc />
    public async Task<LongResp> Add(AddUnitReq req)
    {
        var allCountRes = await _unitRep.GetCount();
        if (allCountRes.IsSuccess() && allCountRes.data > 500)
            return new LongResp().WithResp(RespCodes.OperateObjectExisted, "超过系统支持的最大单位数量(500)!");

        var countRes = await _unitRep.GetCount(req.name);
        if (countRes.IsSuccess()&& countRes.data>0)
            return new LongResp().WithResp(RespCodes.OperateObjectExisted, "当前单位已经存在!");
        
        var mo = req.ToMo();
        await _unitRep.Add(mo);

        return new LongResp(mo.id);
    }
}
