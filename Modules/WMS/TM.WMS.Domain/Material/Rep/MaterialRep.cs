using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Domain;

namespace TM.WMS;

/// <summary>
///  物料 对象仓储
/// </summary>
public class MaterialRep : BaseWMSRep<MaterialMo,long> 
{
    /// <inheritdoc />
    public MaterialRep() : base(WMSConst.Tables.material)
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<MaterialMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
    }

    /// <inheritdoc />
    protected override string BuildSimpleSearch_FilterItemSql(string key, string value, Dictionary<string, object> sqlParas)
    {
        switch (key)
        {
            case "code":
                sqlParas.Add("@code",value);
                return " t.`code`=@code ";

            case "type":
                if (value=="-999")
                    return string.Empty;

                sqlParas.Add("@type", value);
                return " t.`type`=@type ";

            case "name":
                return $" t.`name` like '%{SqlFilter(value)}%' ";

            case "c_id":
                sqlParas.Add("@c_id", value);
                return " t.`c_id`=@c_id ";
        }
        return base.BuildSimpleSearch_FilterItemSql(key, value, sqlParas);
    }

    /// <summary>
    /// 获取已有单位数量
    /// </summary>
    /// <returns></returns>
    public Task<Resp<int>> GetCount()
    {
        var tenantId = CoreContext.GetTenantLongId();

        // 包含已经作废的数据
        var sql = $"SELECT count(0) FROM {TableName} t where t.tenant_id=@tenant_id ";

        sql = string.Concat(sql, " and t.code=@code");
        return Get<int>(sql, new { tenant_id = tenantId});
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
        return Update(u => new {u.status}, w => w.id == id && w.tenant_id== tenantId, new {status});
    }

    /// <summary>
    ///  修改物料信息
    /// </summary>
    /// <param name="mo"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<Resp> Update(long id, MaterialMo mo)
    {
        var tenantId = CoreContext.GetTenantLongId();

        return Update(u => new {u.name, u.c_id, u.type, u.factory_serial, u.tec_spec, u.multi_units, u.remark}
            , w => w.id == id && w.tenant_id == tenantId
            , mo);
    }
}
