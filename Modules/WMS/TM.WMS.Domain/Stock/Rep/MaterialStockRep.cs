using System.Text;
using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;

namespace TM.WMS;

/// <summary>
///  物料库存（物料维度
/// </summary>
public class MaterialStockRep : BaseWMSRep<AreaStockMo, long>
{

    /// <inheritdoc />
    public MaterialStockRep() : base(WMSConst.Tables.area_stock)
    {
    }



    #region 聚合关联物料表查询

    
    /// <summary>
    ///  查询列表
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<PageList<MaterialStockView>> SearchUnionGroup(SearchReq req)
    {
        var tenantId = CoreContext.GetTenantLongId();
        var paras    = new Dictionary<string, object> {{"@tenant_id", tenantId}};

        const string orderBy = "ORDER BY ma.id DESC ";
        const string groupBy = "group by ma.id";

        const string colNames = @"ma.`id` material_id,ma.`name`,ma.`code`,ma.`type`,ma.`basic_unit`,
        count(ast.`area_id`) area_count,
        count(ast.`batch_id`) batch_count,
        sum(ast.`usable_count`) usable_count";

        var tableSql = GetUnionTableSql(req);
        var whereSql = GetUnionWhereSql(req, paras);
        var pageSql  = $" limit {req.size} offset {req.GetStartRow()} ";

        var selectSql = $"SELECT {colNames} FROM {tableSql} {whereSql} {groupBy} {orderBy} {pageSql}";
        var totalSql  = req.req_count ? @$"SELECT count(0) FROM {tableSql} {whereSql} {groupBy}" : null;

        return GetPageList<MaterialStockView>(selectSql, paras, totalSql);
    }
    
    private static string GetUnionTableSql(SearchReq req)
    {
        return $"{WMSConst.Tables.material} ma inner join {WMSConst.Tables.area_stock} ast on  ma.id=ast.material_id and ma.tenant_id=@tenant_id and ast.tenant_id=@tenant_id";
    }

    private static string GetUnionWhereSql(SearchReq req, Dictionary<string,object> paras)
    {
        var whereSql = new StringBuilder("where 1=1");
        foreach (var f in req.filter)
        {
            switch (f.Key)
            {
                case "warehouse_id":
                    whereSql.Append(" and ").Append(FormatWarehouseSql(f.Value.ToInt64(), 0, paras));
                    break;

                case "batch_ids":
                    // 经过一层转化，防止外部注入
                    var ids = f.Value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(i => i.ToInt64());

                    whereSql.Append($" and ast.`batch_id` in ({string.Join(",", ids)})");
                    break;

                case "code":

                    paras.Add($"@material_code", f.Value);
                    whereSql.Append(" and ma.`code` =@material_code");

                    break;

                case "name":
                    
                    whereSql.Append($" ma.`name` like '%{SqlFilter(f.Value)}%' ");

                    break;

                case "c_id":
                    paras.Add("@c_id", f.Value.ToInt64());
                    return " ma.`c_id`=@c_id ";
            }
        }
        return whereSql.ToString();
    }


    private static string FormatWarehouseSql(long wId, int index, Dictionary<string, object> paras)
    {
        if (wId <= 0)
            return string.Empty;
        
        var isRootHouse = !wId.ToString().TrimEnd('0').Contains("0");

        if (isRootHouse)
        {
            var range = TreeNumHelper.FormatSubNumRange(wId);
            paras.Add($"@min_warehouse_id{index}", range.minSubNum);
            paras.Add($"@max_warehouse_id{index}", range.maxSubNum);
            return $" ( ast.`warehouse_id`>=@min_warehouse_id{index} and ast.`warehouse_id`<=@max_warehouse_id{index}) ";
        }

        paras.Add($"@warehouse_id{index}", wId);
        return $" ast.`warehouse_id`=@warehouse_id{index} ";
    }

    #endregion
    


    /// <summary>
    ///  获取库存聚合信息
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    public Task<List<MaterialStockCount>> GetStockList(GetStockListReq req)
    {
        var paras = new Dictionary<string, object>();
        var selectSql =
            $"Select t.material_id,sum(t.`usable_count`) usable_count from  {TableName} {FormatStockGroupWhereSql(req, paras)}";

        return GetList<MaterialStockCount>(selectSql, paras);
    }

    private string FormatStockGroupWhereSql(GetStockListReq req, Dictionary<string, object> paras)
    {
        var where = new StringBuilder(" where");
        if (req.m_ids.Count == 1)
        {
            paras.Add("@material_id", req.m_ids[0]);
            where.Append(" t.`material_id`=@material_id");
        }
        else
        {
            where.Append(" t.`material_id` in (").Append(string.Join(',', req.m_ids)).Append(')');
        }

        if (req.warehouse_id > 0)
        {
            where.Append(FormatWarehouseSql(req.warehouse_id, 0, paras));
        }

        return where.ToString();
    }


}
