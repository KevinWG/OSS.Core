using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Domain;

namespace TM.WMS;

/// <summary>
///  MaterialCategory 对象仓储
/// </summary>
public class MaterialCategoryRep : BaseWMSRep<MCategoryMo,long> 
{
    /// <inheritdoc />
    public MaterialCategoryRep() : base("wms_material_category")
    {
    }

    /// <summary>
    ///  获取当前节点下的最后一个子节点
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public Task<Resp<long>> GetLastSubNum(long parentId)
    {
        var tenantId = CoreContext.GetTenantLongId();

        // 包含已经被软删除的数据
        var sql =
            $"SELECT id FROM {TableName} t where t.parent_id=@parent_id and tenant_id=@tenant_id order by t.id desc limit 1";
        return Get<long>(sql, new { parent_id = parentId, tenant_id = tenantId });
    }

    /// <summary>
    ///  获取指定编码分类信息
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public Task<Resp<int>> GetCountByParent(long parentId)
    {
        var tenantId = CoreContext.GetTenantLongId();

        var sql =
            $"SELECT count(0) count FROM {TableName} t where t.parent_id=@parent_id and tenant_id=@tenant_id and status>@status";
        return Get<int>(sql, new
        {
            parent_id = parentId,
            tenant_id = tenantId,
            status = CommonStatus.UnActive
        });
    }

    /// <summary>
    ///  分类搜索
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<MCategoryMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
    }

    /// <inheritdoc />
    protected override string BuildSimpleSearch_OrderSql(Dictionary<string, SortType> orders)
    {
        return " ORDER BY t.order desc,t.id desc";
    }

    /// <summary>
    ///   修改分类状态
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
    ///   编辑分类信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<Resp> UpdateName(long id, UpdateMCategoryNameReq req)
    {
        var tenantId = CoreContext.GetTenantLongId();
        return Update(u => new { u.name }, w => w.id == id && w.tenant_id == tenantId, new { name = req.name });
    }

    /// <summary>
    ///  修改分类排序
    /// </summary>
    /// <param name="id"></param>
    /// <param name="order"></param>
    /// <returns></returns>
    public Task<Resp> UpdateOrder(long id, int order)
    {
        var tenantId = CoreContext.GetTenantLongId();
        return Update(u => new { u.order }, w => w.id == id && w.tenant_id == tenantId, new { order = order });
    }
}
