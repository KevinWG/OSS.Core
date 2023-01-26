using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;
using OSS.Tools.Cache;

namespace TM.WMS;


/// <summary>
///  仓库 服务
/// </summary>
public class WarehouseService : IWarehouseService
{
    private static readonly WarehouseRep _warehouseRep = new();

    private const int _maxWarehouseCount = 200;
    private const int _maxWareAreaCount = 200;

    /// <inheritdoc />
    public async Task<ListResp<WarehouseFlatItem>> AllUseable()
    {
        var listRes = await All();
        return listRes.IsSuccess()
            ? new ListResp<WarehouseFlatItem>(listRes.data.Where(w => w.status == CommonStatus.Original).ToList())
            : listRes;
    }

    /// <inheritdoc />
    public Task<ListResp<WarehouseFlatItem>> All()
    {
        var getFunc  = GetAll;
        return getFunc.WithRespCacheAsync(WMSConst.CacheKeys.WMS_Warehouse_All,TimeSpan.FromHours(1));
    }

    public async Task<Resp<WarehouseFlatItem>> GetUseableFlatItem(long id)
    {
        var warehouseRes = await GetUseable(id);
        if (!warehouseRes.IsSuccess())
            return new Resp<WarehouseFlatItem>().WithResp(warehouseRes);

        var flatItem = warehouseRes.data.ToItem<WarehouseFlatItem>();

        if (flatItem.parent_id>0)
        {
            var parentRes = await GetUseable(flatItem.parent_id);
            if (!parentRes.IsSuccess())
                return new Resp<WarehouseFlatItem>().WithResp(warehouseRes);

            flatItem.parent_name = parentRes.data.name;
        }

        return new Resp<WarehouseFlatItem>(flatItem);
    }

    private async Task<ListResp<WarehouseFlatItem>> GetAll()
    {
        var req = new SearchReq() { size = _maxWarehouseCount, req_count = false };

        var pageList = await _warehouseRep.Search(req);
        var wList = pageList.data;
        if (!wList.Any())
            return new ListResp<WarehouseFlatItem>(new List<WarehouseFlatItem>(0));

        var list = new List<WarehouseFlatItem>(wList.Count);

        foreach (var warehouseMo in wList)
        {
            var item = warehouseMo.ToItem<WarehouseFlatItem>();

            if (warehouseMo.parent_id > 0)
                item.parent_name = wList.FirstOrDefault(w => w.id == item.parent_id)?.name ?? string.Empty;

            list.Add(item);
        }

        return new ListResp<WarehouseFlatItem>(list);
    }


    /// <inheritdoc />
    public async Task<Resp> SetUseable(long id, ushort flag)
    {
        var wRes = await Get(id);
        if (!wRes.IsSuccess())
            return wRes;

        if (flag == 0)
        {
            var countRes = await _areaRep.GetUseableCount(id);
            if (!countRes.IsSuccess())
                return new Resp(RespCodes.OperateFailed, "执行区位校验错误！");

            if (countRes.data > 0)
                return new Resp(RespCodes.OperateObjectExisted, "仓库存在有效的区位，不可禁用！");
        }

        if (wRes.data.parent_id == 0) // 最多两级，只需要判断第一层级即可
        {
            var wCountRes = await _warehouseRep.GetUseableCount(id);
            if (wCountRes.IsSuccess() && wCountRes.data > 0)
                return new Resp(RespCodes.OperateObjectExisted, "存在有效的子级仓库，不可禁用！");
        }

        return await _warehouseRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive).WithRespCacheClearAsync(WMSConst.CacheKeys.WMS_Warehouse_All);
    }

    /// <inheritdoc />
    public async Task<LongResp> Add(AddWarehouseReq req)
    {
        if (req.parent_id > 0)
        {
            var parentRes = await GetUseable(req.parent_id);
            if (!parentRes.IsSuccess())
                return new LongResp().WithResp(parentRes);
            if (parentRes.data.parent_id > 0)
                return new LongResp().WithResp(RespCodes.OperateFailed, "仓库层级不能超过两级!");
        }

        var countRes = await _warehouseRep.GetCount();
        if (!countRes.IsSuccess() || countRes.data > _maxWarehouseCount)
            return new LongResp().WithResp(RespCodes.OperateFailed, $"添加仓库失败（仓库数量最多{_maxWarehouseCount}个）");

        var newIdRes = await GetNewId(req.parent_id);
        if (!newIdRes.IsSuccess())
            return newIdRes;

        var warehouse = req.MapToWarehouseMo(newIdRes.data);

        await _warehouseRep.Add(warehouse);

        await CacheHelper.RemoveAsync(WMSConst.CacheKeys.WMS_Warehouse_All);

        return new LongResp(warehouse.id);
    }


    /// <inheritdoc />
    public Task<Resp> Update(long id, UpdateWarehouseReq req)
    {
        return _warehouseRep.Update(id,req).WithRespCacheClearAsync(WMSConst.CacheKeys.WMS_Warehouse_All);
    }


    #region 辅助方法


    private static async Task<LongResp> GetNewId(long parentId)
    {
        var maxNumRes = await _warehouseRep.GetLastSubNum(parentId);

        if (!maxNumRes.IsSuccess())
            return new LongResp().WithResp(RespCodes.OperateFailed, "查询仓库树形编码出现错误!");

        // 生成树形编码
        var maxSubId = maxNumRes.data;
        long treeNum;
        try
        {
            treeNum = TreeNumHelper.GenerateSmallNum(parentId, maxSubId);
        }
        catch (Exception)
        {
            return new LongResp().WithResp(RespCodes.OperateFailed, "无法生成有效的仓库编码，可能已经超出长度限制");
        }

        return new LongResp(treeNum);
    }

    
    private async Task<Resp<WarehouseMo>> Get(long id)
    {
        var getRes = await _warehouseRep.GetById(id);
        return getRes.IsSuccess() ? getRes : new Resp<WarehouseMo>().WithResp(getRes, "未能找到仓库信息");
    }

    private async Task<IResp<WarehouseMo>> GetUseable(long id)
    {
        var getRes = await Get(id);
        if (!getRes.IsSuccess())
            return getRes;

        return getRes.data.status >= 0
            ? getRes
            : new Resp<WarehouseMo>().WithResp(RespCodes.OperateObjectNull, "未能找到有效可用的仓库信息");
    }




    #endregion

    private static readonly WareAreaRep _areaRep = new();

    /// <inheritdoc />
    public async Task<LongResp> AddArea(AddAreaReq req)
    {
        var houseRes = await GetUseable(req.warehouse_id);
        if (!houseRes.IsSuccess())
            return new LongResp().WithResp(houseRes);

        var countRes = await _areaRep.GetCount(req.warehouse_id);
        if (!countRes.IsSuccess() || countRes.data > _maxWareAreaCount)
            return new LongResp().WithResp(RespCodes.OperateFailed, $"添加 库区/位 失败（仓库下 区/位 数量最多{_maxWareAreaCount}个）");

        var checkRes = await CheckAreaCode(new CheckAreaCodeReq() { w_id = req.warehouse_id, code = req.code });
        if (!checkRes.IsSuccess())
            return new LongResp().WithResp(checkRes);

        var mo = req.MapToWareAreaMo();

        await _areaRep.Add(mo);
        return new LongResp(mo.id);
    }

    /// <inheritdoc />
    public Task<Resp> UpdateArea(long id, UpdateAreaReq req)
    {
        return _areaRep.Update(id, req);
    }

    /// <summary>
    ///  验证库区/位 编码是否可以添加
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public async Task<Resp> CheckAreaCode(CheckAreaCodeReq req)
    {
        var countRes = await _areaRep.GetCount(req.w_id, req.code);
        if (countRes.IsSuccess() && countRes.data == 0)
            return new Resp();

        return new Resp(RespCodes.OperateObjectExisted, "当前区/位编码已经存在!");
    }

    /// <inheritdoc />
    public async Task<ListResp<WareAreaMo>> AreaList(long w_id)
    {
        var list = await _areaRep.GetList(w_id);
        return new ListResp<WareAreaMo>(list);
    }

    /// <inheritdoc />
    public Task<Resp> SetAreaUseable(long id, ushort flag)
    {
        // todo 添加对库存的判断，禁用时必须无有效库存
        return _areaRep.UpdateStatus(id, flag == 1 ? CommonStatus.Original : CommonStatus.UnActive);
    }

    /// <inheritdoc />
    public Task<Resp> SetAreaTradeFlag(long id, TradeFlag flag)
    {
        return _areaRep.UpdateTradeFlag(id, flag);
    }

    /// <inheritdoc />
    async Task<Resp<WareAreaMo>> IWarehouseService.GetUseableArea(long id)
    {
        var getRes = await _areaRep.GetById(id);
        if (!getRes.IsSuccess())
            return getRes.WithErrMsg("未能找到区位信息!");

        return getRes.data.status >= 0
            ? getRes
            : new Resp<WareAreaMo>().WithResp(RespCodes.OperateObjectNull, "未能找到有效可用的区位信息！");
    }

    /// <inheritdoc />
    Task<List<WareAreaMo>> IWarehouseService.GetAreaList(List<long> areaIdList)
    {
        return _areaRep.GetByIds(areaIdList);
    }
}




internal interface IWarehouseService : IWarehouseOpenService
{
    /// <summary>
    ///  获取有效的区位信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    internal Task<Resp<WareAreaMo>> GetUseableArea(long id);

    /// <summary>
    ///  获取指定区位信息
    /// </summary>
    /// <param name="areaIdList"></param>
    /// <returns></returns>
    internal Task<List<WareAreaMo>> GetAreaList(List<long> areaIdList);
}