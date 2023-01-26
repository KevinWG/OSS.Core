using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace TM.WMS;

internal interface IMaterialService : IMaterialOpenService
{
}


/// <summary>
///  物料 服务
/// </summary>
public class MaterialService : IMaterialService
{
    private static readonly MaterialRep _materialRep = new();

    /// <inheritdoc />
    public async Task<PageListResp<MaterialView>> MSearch(SearchReq req)
    {
        var pageList = await _materialRep.Search(req);
        return new PageListResp<MaterialView>(pageList.total, pageList.data.Select(m => m.ToView()).ToList());
    }

    /// <inheritdoc />
    public async Task<Resp<MaterialView>> Get(long id)
    {
        var getRes = await _materialRep.GetById(id);
        return getRes.IsSuccess()
            ? new Resp<MaterialView>(getRes.data.ToView())
            : new Resp<MaterialView>().WithResp(getRes, "未能找到物料信息");
    }




    /// <inheritdoc />
    public async Task<Resp<MaterialView>> GetUseable(long id)
    {
        var getRes = await Get(id);
        if (!getRes.IsSuccess())
            return getRes;

        return getRes.data.status >= 0
            ? getRes
            : new Resp<MaterialView>().WithResp(RespCodes.OperateObjectNull, "未能找到有效可用的物料信息");
    }

    /// <inheritdoc />
    public Task<Resp> SetUseable(long id, ushort flag)
    {
        return _materialRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<LongResp> Add(AddMaterialReq req)
    {
        var mo = req.ToMo();

        if (string.IsNullOrEmpty(mo.code))
        {
            // todo  更换规则
            mo.code = mo.id.ToString();
        }

        var checkRes = CheckMultiUnit(req.multi_units, req.basic_unit);
        if (!checkRes.IsSuccess())
            return new LongResp().WithResp(checkRes);

        await _materialRep.Add(mo);
        return new LongResp(mo.id);
    }

    /// <inheritdoc />
    public async Task<Resp> Update(long id, UpdateMaterialReq req)
    {

        var mRes = await _materialRep.GetById(id);
        if (!mRes.IsSuccess())
            return mRes;
        var m = mRes.data;

        var checkRes = CheckMultiUnit(req.multi_units, m.basic_unit);
        if (!checkRes.IsSuccess())
            return checkRes;


        m.FormatByUpdateReq(req);

        return await _materialRep.Update(id, m);
    }


    private static Resp CheckMultiUnit(IList<MultiUnitItem>? items, string basicUnit)
    {
        if (items == null)
            return new Resp();

        var repeat = items.GroupBy(u => u.unit).Count(g => g.Count() > 1);
        if (repeat > 0)
        {
            return new Resp(RespCodes.OperateFailed, "多单位存在重复的项");
        }

        if (items.Any(u => string.IsNullOrEmpty(u.unit) || u.unit == basicUnit))
        {
            return new Resp(RespCodes.OperateFailed, "多单位中存在 空值 或者 和标准(最小)单位重复的项");
        }

        return new Resp();
    }
}
