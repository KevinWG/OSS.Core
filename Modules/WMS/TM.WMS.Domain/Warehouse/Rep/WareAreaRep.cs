using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Domain;

namespace TM.WMS;

/// <summary>
///  库位/区 对象仓储
/// </summary>
public class WareAreaRep : BaseWMSRep<WareAreaMo, long>
{
    /// <inheritdoc />
    public WareAreaRep() : base("wms_warearea")
    {
    }


    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="houseId">仓库Id</param>
    /// <returns></returns>
    public Task<List<WareAreaMo>> GetList(long houseId)
    {
        var tenantId = CoreContext.GetTenantLongId();

        return GetList(w => w.warehouse_id == houseId && w.tenant_id == tenantId);
    }

    /// <summary>
    ///   修改状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public Task<Resp> UpdateStatus(long id, CommonStatus status)
    {
        var tenantId = CoreContext.GetTenantLongId();
        return Update(u => new { u.status }, w => w.id == id && w.tenant_id == tenantId, new { status });
    }


    /// <summary>
    ///  获取当前仓库下库位数量(含作废等)
    /// </summary>
    /// <returns></returns>
    public Task<Resp<int>> GetCount(long houseId, string? code = null)
    {
        var tenantId = CoreContext.GetTenantLongId();

        // 包含已经作废的数据
        var sql = $"SELECT count(0) FROM {TableName} t where t.warehouse_id=@warehouse_id and t.tenant_id=@tenant_id";
        if (string.IsNullOrEmpty(code))
            return Get<int>(sql, new { tenant_id = tenantId, warehouse_id = houseId });

        sql = string.Concat(sql, " and t.code=@code");
        return Get<int>(sql, new { tenant_id = tenantId, warehouse_id = houseId, code = code });
    }

    /// <summary>
    ///  获取有效区位数量
    /// </summary>
    /// <param name="warehouseId"></param>
    /// <returns></returns>
    public Task<Resp<int>> GetUseableCount(long warehouseId)
    {
        var tenantId = CoreContext.GetTenantLongId();

        // 包含已经作废的数据
        var sql =
            $"SELECT count(0) FROM {TableName} t where t.warehouse_id=@warehouse_id and t.tenant_id=@tenant_id and t.status>=0";
        return Get<int>(sql, new { tenant_id = tenantId, warehouse_id = warehouseId });
    }

    /// <summary>
    ///    修改交易标识
    /// </summary>
    /// <param name="id"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    public Task<Resp> UpdateTradeFlag(long id, TradeFlag flag)
    {
        var tenantId = CoreContext.GetTenantLongId();
        return Update(u => new { trade_flag = flag }, w => w.id == id && w.tenant_id == tenantId);
    }

    /// <summary>
    ///  修改信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<Resp> Update(long id, UpdateAreaReq req)
    {
        var tenantId = CoreContext.GetTenantLongId();
        return Update(u => new { remark = req.remark }, w => w.id == id && w.tenant_id == tenantId);
    }


    /// <summary>
    ///  通过Id列表获取区位列表
    /// </summary>
    /// <param name="areaIdList"></param>
    /// <returns></returns>
    public  Task<List<WareAreaMo>> GetByIds(List<long>? areaIdList)
    {
        if (areaIdList == null || areaIdList.Count == 0)
            return Task.FromResult(new List<WareAreaMo>());
        
        var tenantId  = CoreContext.GetTenantLongId();
        var selectSql = $"select * from {TableName} where tenant_id=@tenant_id and id in ({string.Join(',', areaIdList)})";

        return GetList(selectSql, new {tenant_id = tenantId});
    }
}
