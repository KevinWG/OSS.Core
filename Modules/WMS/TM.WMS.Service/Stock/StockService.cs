using OSS.Common;
using OSS.Common.Resp;
using System.Transactions;

namespace TM.WMS;


/// <summary>
///  库存 服务
/// </summary>
public class StockService : IStockOpenService
{

    private static readonly MaterialStockRep _mStockRep    = new();
    private static readonly AreaStockRep     _areaStockRep = new();
    private static readonly StockRecordRep   _recordRep    = new();

    #region 物料库存 （物料维度

    /// <inheritdoc />
    public async Task<PageListResp<MaterialStockView>> MSearchUnion(SearchReq req)
    {
        var pagelist = await _mStockRep.SearchUnionGroup(req);
        return new PageListResp<MaterialStockView>(pagelist);
    }

    /// <inheritdoc />
    public async Task<ListResp<MaterialStockCount>> GetStockList(GetStockListReq req)
    {
        if (req.m_ids == null || req.m_ids.Count == 0)
            return new ListResp<MaterialStockCount>(new List<MaterialStockCount>());

        var list = await _mStockRep.GetStockList(req);
        return new ListResp<MaterialStockCount>(list);
    }

    #endregion


    #region 区位物料库存 （区位维度）


    /// <inheritdoc />
    public async Task<PageListResp<AreaStockView>> SearchArea(SearchReq req)
    {
        var pageList = await _areaStockRep.Search(req);
        if (pageList.data.Count == 0)
            return new PageListResp<AreaStockView>(pageList.total, null);

        var areaIdList = pageList.data.Select(a => a.area_id).ToList();

        #region 获取相关仓库和区位信息

        var warehouseListTask = InsContainer<IWarehouseService>.Instance.All();
        var areaListTask      = InsContainer<IWarehouseService>.Instance.GetAreaList(areaIdList);

        Task.WaitAll(warehouseListTask, areaListTask);

        var warehouseList = warehouseListTask.Result.data;
        var areaList      = areaListTask.Result;

        #endregion

        var viewList = pageList.data.Select(x => FormatToView(x, warehouseList, areaList)).ToList();

        return new PageListResp<AreaStockView>(pageList.total, viewList);
    }


    private static AreaStockView FormatToView(AreaStockMo mo, IList<WarehouseFlatItem> warehouses,
                                              IList<WareAreaMo> areas)
    {
        var view = mo.ToView();

        var warehouse = warehouses.FirstOrDefault(w => w.id == view.warehouse_id);
        view.warehouse_name = warehouse == null ? string.Empty : warehouse.full_name;

        var area = areas.FirstOrDefault(a => a.id == view.area_id);
        view.area_code = area == null ? string.Empty : area.code;

        return view;
    }

    #endregion



    #region 修改库存

    /// <inheritdoc />
    public async Task<Resp> StockChange(StockChangeReq req)
    {
        #region 关联实体初始化及验证

        if (req.area_id <= 0 || req.material_id <= 0)
            return new Resp(RespCodes.ParaError, "请指定具体的物料和仓储(区位)信息");

        var wareareaRes = await InsContainer<IWarehouseService>.Instance.GetUseableArea(req.area_id);
        if (!wareareaRes.IsSuccess())
            return new LongResp().WithResp(wareareaRes);

        var warearea = wareareaRes.data;
        if (warearea.trade_flag == TradeFlag.UnActive && req.b_type.IsTradeType())
            return new Resp(RespCodes.ParaError, "当前仓库区位物料不可进行交易!");

        // 获取对应物料信息
        var materialRes = await InsContainer<IMaterialService>.Instance.GetUseable(req.material_id);
        if (!materialRes.IsSuccess())
            return new LongResp().WithResp(materialRes);

        // 获取当前库存信息
        var areaStockRes = await Get(req.material_id, req.area_id, req.batch_id);
        var isNewStock   = areaStockRes.IsRespCode(RespCodes.OperateObjectNull);
        if (!areaStockRes.IsSuccess() && !isNewStock)
            return new LongResp().WithResp(areaStockRes);

        #endregion

        var material  = materialRes.data;
        var areaStock = areaStockRes.data;

        // 初始化变动记录信息
        var                stockRecord = req.ToRecord();
        AreaStockChangeReq stockChange;

        stockRecord.warehouse_id = warearea.warehouse_id;

        FormatMaterialName(stockRecord, material);
        FormatUnitCount(stockRecord, material);

        if (!isNewStock)
        {
            stockRecord.before_basic_count = areaStock.usable_count;
            stockRecord.area_stock_id      = areaStock.id;

            stockChange = new AreaStockChangeReq()
            {
                update_item = new AreaStockUpdateReq()
                    {stock_id = areaStock.id, change_basic_count = stockRecord.change_basic_count}
            };
        }
        else
        {
            var newAreaStock = stockRecord.ToStock(material);
            stockRecord.area_stock_id = newAreaStock.id;

            stockChange = new AreaStockChangeReq()
            {
                is_add   = true,
                add_item = newAreaStock
            };
        }

        // 执行数据库层面修改，保证数据层面的一致性
        using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await ChangeAreaStock(stockChange);
            await _recordRep.Add(stockRecord);

            ts.Complete();
        }

        return new Resp();
    }


    private static Task ChangeAreaStock(AreaStockChangeReq change)
    {
        return change.is_add
            ? _areaStockRep.Add(change.add_item)
            : _areaStockRep.UpdateCount(change.update_item.stock_id, change.update_item.change_basic_count);
    }


    #endregion

    private async Task<Resp<AreaStockMo>> Get(long m_id, long area_id, long batch_id)
    {
        var getRes = await _areaStockRep.Get(m_id, area_id, batch_id);
        return getRes.IsSuccess() ? getRes : new Resp<AreaStockMo>().WithResp(getRes, "未能找到库存信息");
    }

    #region 辅助赋值方法


    private static void FormatMaterialName(StockRecordMo record, MaterialView mView)
    {
        record.material_code = mView.code;
        record.material_name = mView.name;
    }

    private static void FormatUnitCount(StockRecordMo record, MaterialView mView)
    {
        if (record.change_unit == mView.basic_unit)
        {
            record.change_basic_count = record.change_count;
            return;
        }

        var unitItem = mView.multi_units?.FirstOrDefault(u => u.unit == record.change_unit);
        if (unitItem == null)
            throw new RespNotImplementException($"未能在当前物料多单位中查到({record.change_unit}) 信息");

        record.change_basic_count = unitItem.ratio * record.change_count;
    }

    #endregion
}
