using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal;

/// <summary>
///  组织机构 对象仓储
/// </summary>
public class OrganizationRep : BasePortalRep<OrganizationMo>, IOrganizationRep
{
    /// <inheritdoc />
    public OrganizationRep() : base("portal_org")
    {
    }

    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<OrganizationMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
    }


    protected override string BuildSimpleSearch_FilterItemSql(string key, string value, Dictionary<string, object> sqlParas)
    {
        switch (key)
        {
            case "name":
                return $" t.name like '%{SqlFilter(value)}%' ";
            case "org_type":
                sqlParas.Add("@org_type", value.ToInt32());
                return " t.org_type=@org_type ";
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
}
