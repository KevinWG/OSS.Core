using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Rep.Mysql;
using OSS.Tools.Cache;

namespace OSS.Core.Comp.DirConfig.Mysql;

internal class ConfigRep : BaseMysqlRep<ConfigMo, long>
{
    private readonly ConnectionOption _conOpt;

    public ConfigRep(ConnectionOption option) : base(option.WriteConnection, option.ReadConnection, option.TableName)
    {
        _conOpt = option;
    }


    public Task<IResp> UpdateVal(string listKey, string itemKey, string confJson)
    {
        var haveTenantId = CoreContext.Tenant.IsAuthenticated;

        const string updateCols = "item_val=@item_val,status=0";
        var          strWhere   = " where list_key=@list_key and item_key=@item_key";

        var paras = new Dictionary<string, object>
        {
            {"@item_val", confJson},
            {"@list_key", listKey},
            {"@item_key", itemKey}
        };

        if (haveTenantId)
        {
            strWhere = string.Concat(strWhere, " and tenant_id=@tenant_id");
            paras.Add("@tenant_id", CoreContext.Tenant.Identity.id);
        }

        var itemCacheKey = string.Format(System_ConfigItem_ByKey, listKey, itemKey);

        return Update(updateCols, strWhere, paras)
            .WithRespCacheClearAsync(itemCacheKey);
    }


    private const string System_ConfigItem_ByKey = "System_Config_{0}_{1}";

    public Task<IResp<ConfigMo>> GetByKey(string listKey, string itemKey)
    {
        var haveTenantId = CoreContext.Tenant.IsAuthenticated;

        var paras = new Dictionary<string, object>
        {
            {"@list_key", listKey},
            {"@item_key", itemKey}
        };

        var strGetSql = $" select * from {TableName} where list_key=@list_key and item_key=@item_key";
        if (haveTenantId)
        {
            strGetSql = string.Concat(strGetSql, " and tenant_id=@tenant_id");
            paras.Add("@tenant_id", CoreContext.Tenant.Identity.id);
        }

        var itemCacheKey = string.Format(System_ConfigItem_ByKey, listKey, itemKey);

        var getfunc = () => Get<ConfigMo>(strGetSql, paras);

        return getfunc.WithRespCacheAsync(itemCacheKey, TimeSpan.FromHours(1));
    }


    public Task<List<ConfigMo>> GetListByKey(string listKey)
    {
        var haveTenantId = CoreContext.Tenant.IsAuthenticated;

        var paras = new Dictionary<string, object>
        {
            {"@list_key", listKey}
        };

        var strGetSql = $" select * from {TableName} where list_key=@list_key";
        if (haveTenantId)
        {
            strGetSql = string.Concat(strGetSql, " and tenant_id=@tenant_id");
            paras.Add("@tenant_id", CoreContext.Tenant.Identity.id);
        }

        return GetList(strGetSql, paras);
    }

    public async Task<int> GetCount(string listKey)
    {
        var haveTenantId = CoreContext.Tenant.IsAuthenticated;

        var paras = new Dictionary<string, object>
        {
            {"@list_key", listKey}
        };

        var strGetSql = $" select count(0) from {TableName} where list_key=@list_key";
        if (haveTenantId)
        {
            strGetSql = string.Concat(strGetSql, " and tenant_id=@tenant_id");
            paras.Add("@tenant_id", CoreContext.Tenant.Identity.id);
        }

        return (await Get<int>(strGetSql, paras)).data;
    }
}