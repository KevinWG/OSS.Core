using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Domain;

namespace TM.WMS;

/// <summary>
///  出入库申请 对象仓储
/// </summary>
public class StockApplyRep : BaseWMSRep<StockApplyMo, long>
{
    /// <inheritdoc />
    public StockApplyRep() : base("wms_stock_apply")
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<StockApplyMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
    }


    /// <inheritdoc />
    protected override string BuildSimpleSearch_FilterItemSql(string key, string value,
                                                              Dictionary<string, object> sqlParas)
    {
        switch (key)
        {
            case "title":
                return $" t.`title` like '%{SqlFilter(value)}%' ";

            case "direction":
                sqlParas.Add("@direction", value.ToInt32());
                return " t.`direction`=@direction ";
        }

        return base.BuildSimpleSearch_FilterItemSql(key, value, sqlParas);
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
        return Update(u => new {u.status}, w => w.id == id && w.tenant_id == tenantId, new {status});
    }

    /// <summary>
    ///  更新申请信息
    /// </summary>
    /// <param name="apply"></param>
    /// <returns></returns>
    public Task<Resp> Edit(StockApplyMo apply)
    {
        var tenantId = CoreContext.GetTenantLongId();

        return Update(u => new
        {
            u.title,
            u.warehouse_id,
            u.warehouse_name,
            u.direction,
            u.org_id, u.org_name,
            u.plan_time
        }, w => w.id == apply.id && w.tenant_id == tenantId, apply);
    }
}
