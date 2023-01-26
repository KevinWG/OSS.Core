using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;

namespace TM.WMS;

/// <summary>
///  库存 对象仓储
/// </summary>
public class AreaStockRep : BaseWMSRep<AreaStockMo, long>
{
    /// <inheritdoc />
    public AreaStockRep() : base(WMSConst.Tables.area_stock)
    {
    }


    /// <summary>
    ///  搜索
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public  Task<PageList<AreaStockMo>> Search(SearchReq req)
    {
        return SimpleSearch(req);
    }





    /// <inheritdoc />
    protected override string BuildSimpleSearch_FilterItemSql(string key, string value, Dictionary<string, object> sqlParas)
    {
        switch (key)
        {
            case "material_id":
                sqlParas.Add("@material_id", value.ToInt64());
                return " t.`material_id`=@material_id";
            case "warehouse_id":
                return FormatWarehouseSql(value.ToInt64(), 0, sqlParas);

            case "area_id":
                sqlParas.Add("@area_id", value.ToInt64());
                return " t.`area_id`=@area_id";
        }
        return base.BuildSimpleSearch_FilterItemSql(key, value, sqlParas);
    }





    /// <summary>
    /// 获取对应区位，对应批次的物料库存信息
    /// </summary>
    /// <param name="m_id"></param>
    /// <param name="area_id"></param>
    /// <param name="batch_id"></param>
    /// <returns></returns>
    public Task<Resp<AreaStockMo>> Get(long m_id, long area_id, long batch_id)
    {
        var tenantId = CoreContext.GetTenantLongId();
        return Get(w => w.material_id == m_id && w.area_id == area_id
                                              && w.batch_id == batch_id
                                              && w.tenant_id == tenantId);
    }




    /// <summary>
    ///  更新库存数量信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public Task<Resp> UpdateCount(long id,  decimal count)
    {
        const string colNameSql = " usable_count = usable_count + @count ";

        var tenantId = CoreContext.GetTenantLongId();
        return Update(colNameSql, " where id=@id and tenant_id=@tenant_id", new {count, id, tenant_id=tenantId });
    }


    private static string FormatWarehouseSql(long warehouseId, int index, Dictionary<string, object> paras)
    {
        if (warehouseId<=0)
        {
            return string.Empty;
        }
        var isRootHouse = !warehouseId.ToString().TrimEnd('0').Contains("0");

        if (isRootHouse)
        {
            var range = TreeNumHelper.FormatSubNumRange(warehouseId);
            paras.Add($"@min_warehouse_id{index}", range.minSubNum);
            paras.Add($"@max_warehouse_id{index}", range.maxSubNum);
            return $" ( t.`warehouse_id`>=@min_warehouse_id{index} and t.`warehouse_id`<=@max_warehouse_id{index}) ";
        }

        paras.Add($"@warehouse_id{index}", warehouseId);
        return $" t.`warehouse_id`=@warehouse_id{index} ";
    }

}
