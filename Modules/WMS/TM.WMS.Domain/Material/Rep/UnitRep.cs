using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;

namespace TM.WMS;

/// <summary>
///  物料库存单位 对象仓储
/// </summary>
public class UnitRep : BaseWMSRep<UnitMo,long> 
{
    /// <inheritdoc />
    public UnitRep() : base("wms_units")
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<UnitMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
    }

    /// <summary>
    /// 获取所有单位
    /// </summary>
    /// <returns></returns>
    public Task<List<UnitMo>> GetAll()
    {
        var tenantId = CoreContext.GetTenantLongId();
        return GetList(u=>u.tenant_id== tenantId);
    }


    /// <summary>
    ///  获取单位数量
    /// </summary>
    /// <param name="unitName"></param>
    /// <returns></returns>
    public Task<Resp<int>> GetCount(string unitName = "")
    {
        var tenantId = CoreContext.GetTenantLongId();

        // 包含已经作废的数据
        var sql = $"SELECT count(0) FROM {TableName} t where t.tenant_id=@tenant_id";
        if (string.IsNullOrEmpty(unitName))
            return Get<int>(sql, new { tenant_id = tenantId });

        sql = string.Concat(sql, " and t.name=@name");
        return Get<int>(sql, new { tenant_id = tenantId, name = unitName });
    }


    /// <summary>
    ///  通过名称获取
    /// </summary>
    /// <param name="unitName"></param>
    /// <returns></returns>
    public Task<Resp<UnitMo>> GetByName(string unitName)
    {
        var tenantId = CoreContext.GetTenantLongId();
        return Get(u => u.tenant_id == tenantId && u.name == unitName);
    }



}
