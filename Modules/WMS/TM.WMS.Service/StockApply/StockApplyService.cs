using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;
using OSS.Common.Extension;
using OSS.Core.Module.Portal.Client;

namespace TM.WMS;

/// <summary>
///  出入库申请 服务
/// </summary>
public class StockApplyService : IStockApplyOpenService
{
    private static readonly StockApplyRep _applyRep = new();

    /// <inheritdoc />
    public async Task<TokenPageListResp<StockApplyMo>> MSearch(SearchReq req)
    {
        var pageList = await _applyRep.Search(req);
        return pageList.ToTokenPageRespWithIdToken();
    }

    /// <inheritdoc />
    public async Task<Resp<StockApplyMo>> Get(long id)
    {
        var getRes = await _applyRep.GetById(id);
        return getRes.IsSuccess() ? getRes : new Resp<StockApplyMo>().WithResp(getRes, "未能找到出入库申请信息");
    }

    /// <inheritdoc />
    public async Task<Resp<StockApplyMo>> GetUseable(long id)
    {
        var getRes = await Get(id);
        if (!getRes.IsSuccess())
            return getRes;

        return getRes.data.status >= 0
            ? getRes
            : new Resp<StockApplyMo>().WithResp(RespCodes.OperateObjectNull, "未能找到有效可用的出入库申请信息");
    }

    /// <inheritdoc />
    public Task<Resp> SetUseable(string pass_token, ushort flag)
    {
        var id = PassTokenHelper.GetData(pass_token).ToInt64();

        return _applyRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public async Task<LongResp> Add(AddStockApplyReq req)
    {
        var mo = req.MapToStockApplyMo();

        var formatRes = await FormatName(req, mo);
        if (!formatRes.IsSuccess())
            return new LongResp().WithResp(formatRes);

        await _applyRep.Add(mo);
        return new LongResp(mo.id);
    }



    /// <inheritdoc />
    public async Task<Resp> Edit(string pass_token, AddStockApplyReq req)
    {
        var id = PassTokenHelper.GetData(pass_token).ToInt64();

        var applyRes = await GetUseable(id);
        if (!applyRes.IsSuccess())
            return applyRes;

        var apply = applyRes.data;
        apply.FormatByReq(req);

        var formatRes = await FormatName(req, apply);
        if (!formatRes.IsSuccess())
            return new LongResp().WithResp(formatRes);

        return await _applyRep.Edit(apply);
    }

    private static async Task<Resp> FormatName(AddStockApplyReq req, StockApplyMo mo)
    {
        var wareRes = await InsContainer<IWarehouseService>.Instance.GetUseableFlatItem(req.warehouse_id);
        if (!wareRes.IsSuccess())
            return wareRes;

        var orgRes = await PortalRemoteClient.Organization.Get(req.org_id);
        if (!orgRes.IsSuccess())
            return orgRes;

        var warehouse = wareRes.data;
        var org       = orgRes.data;

        mo.org_name       = org.name;
        mo.warehouse_name = warehouse.full_name;

        return new Resp();
    }

}
