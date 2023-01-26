using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Domain;

namespace TM.WMS;

/// <summary>
///  仓库 对象仓储
/// </summary>
public class WarehouseRep : BaseWMSRep<WarehouseMo, long>
{
    /// <inheritdoc />
    public WarehouseRep() : base("wms_warehouse")
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<WarehouseMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
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
    ///  获取当前所有仓库数量
    /// </summary>
    /// <returns></returns>
    public Task<Resp<int>> GetCount()
    {
        var tenantId = CoreContext.GetTenantLongId();

        // 包含已经被软删除的数据
        var sql = $"SELECT count(0) FROM {TableName} t where t.tenant_id=@tenant_id";
        return Get<int>(sql, new {  tenant_id = tenantId });
    }

    /// <summary>
    ///  获取可用仓库数量
    /// </summary>
    /// <returns></returns>
    public Task<Resp<int>> GetUseableCount(long parentId)
    {
        var tenantId = CoreContext.GetTenantLongId();

        // 包含已经被软删除的数据
        var sql = $"SELECT count(0) FROM {TableName} t where t.tenant_id=@tenant_id and t.parent_id=@parent_id and t.status>=0";
        return Get<int>(sql, new { tenant_id = tenantId, parent_id = parentId });
    }

    /// <summary>
    ///   获取当前最大值
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public Task<Resp<long>> GetLastSubNum(long parentId)
    {
        var tenantId = CoreContext.GetTenantLongId();

        // 包含已经被软删除的数据
        var sql =
            $"SELECT id FROM {TableName} t where t.parent_id=@parent_id and t.tenant_id=@tenant_id order by t.id desc limit 1";
        return Get<long>(sql, new { parent_id = parentId, tenant_id = tenantId });
    }

    /// <summary>
    ///  修改仓库信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<Resp> Update(long id, UpdateWarehouseReq req)
    {
        var tenantId = CoreContext.GetTenantLongId();
        return Update(u => new { u.name,u.remark }, w => w.id == id && w.tenant_id == tenantId, req);
    }
}
