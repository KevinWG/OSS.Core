
using MySql.Data.MySqlClient;
using System.Data;

namespace OSS.Core.Rep.Dapper;

/// <summary>
///   mysql仓储基类
/// </summary>
/// <typeparam name="TType"></typeparam>
/// <typeparam name="IdType"></typeparam>
public abstract class BaseMysqlRep<TType, IdType> : BaseRep<TType, IdType>
   // where TType : BaseMo<IdType>
{
    private readonly string _writeConnection;
    private readonly string _readConnection;

    /// <summary>
    ///  构造函数
    /// </summary>
    /// <param name="writableConnection">读写分离的写数据库连接串</param>
    /// <param name="readableConnection">读写分离的读数据库连接串</param>
    /// <param name="tableName">当前仓储表名（通用操作使用）</param>
    protected BaseMysqlRep(string writableConnection, string readableConnection, string tableName) : base(tableName)
    {
        _writeConnection = writableConnection;
        _readConnection  = readableConnection;
    }

    /// <inheritdoc />
    protected override IDbConnection GetDbConnection(bool isWriteOperate)
    {
        return new MySqlConnection(isWriteOperate ? _writeConnection : _readConnection);
    }

    #region 简单搜索封装




    ///// <summary>
    /////  通用搜索
    ///// </summary>
    ///// <returns></returns>
    //protected Task<PageList<TType>> SimpleSearch(SearchReq req)
    //{
    //    return SimpleSearch<TType>(req);
    //}

    ///// <summary>
    /////  通用搜索
    ///// </summary>
    ///// <returns></returns>
    //protected Task<PageList<RType>> SimpleSearch<RType>(SearchReq req)
    //{
    //    var sqlParas = new Dictionary<string, object>(req.filter.Count);

    //    var columns   = BuildSimpleSearch_SelectColumns(req);
    //    var tableName = BuildSimpleSearch_TableName(req, sqlParas);
    //    var whereSql  = BuildSimpleSearch_WhereSql(req.filter, sqlParas);
    //    var orderSql  = BuildSimpleSearch_OrderSql(req.orders);

    //    var offCount = req.GetStartRow();

    //    var totalSql = req.req_count ? $"select count(1) from {tableName} {whereSql}" : string.Empty;
    //    var selectSql = $"select {columns} from {tableName} {whereSql} {orderSql} limit {req.size} offset {offCount}";

    //    return GetPageList<RType>(selectSql, sqlParas, totalSql);
    //}

    ///// <summary>
    /////  构建通用搜索的列名
    ///// </summary>
    ///// <param name="req"></param>
    ///// <returns></returns>
    //protected virtual string BuildSimpleSearch_SelectColumns(SearchReq req)
    //{
    //    return "t.*";
    //}

    ///// <summary>
    /////  构建通用搜索的表名
    ///// </summary>
    ///// <param name="req"></param>
    ///// <param name="sqlParas"></param>
    ///// <returns></returns>
    //protected virtual string BuildSimpleSearch_TableName(SearchReq req, Dictionary<string, object> sqlParas)
    //{
    //    return  string.Concat(TableName," t");
    //}

    ///// <summary>
    ///// 生成 where 语句
    ///// </summary>
    ///// <param name="searchFilters">搜索的条件信息</param>
    ///// <param name="sqlParas">sql语句中的参数信息</param>
    ///// <returns></returns>
    //protected virtual string BuildSimpleSearch_WhereSql(Dictionary<string, string> searchFilters,
    //                                                    Dictionary<string, object> sqlParas)
    //{

    //    if (HaveStatusColumn && !searchFilters.ContainsKey("status"))
    //        searchFilters.Add("status", "-9999");
    //    if (HaveTenantIdColumn && !searchFilters.ContainsKey("tenant_id"))
    //        searchFilters.Add("tenant_id",CoreContext.Tenant.Identity.id );

    //    var strBuilder = new StringBuilder("where 1=1 ");
    //    foreach (var filter in searchFilters)
    //    {
    //        if (string.IsNullOrEmpty(filter.Value))
    //        {
    //            continue;
    //        }

    //        // 使用case映射， 防止其他未预料的 key 值，或者外部攻击的字段脚本
    //        var itemSql = BuildSimpleSearch_FilterItemSql(filter.Key, filter.Value, sqlParas);
    //        if (string.IsNullOrEmpty(itemSql))
    //            continue;

    //        strBuilder.Append(" and ");
    //        strBuilder.Append(itemSql);
    //    }

    //    return strBuilder.ToString();
    //}


    ///// <summary>
    ///// 生成 where 条件子语句 -- 针对每个单独的 filter item
    /////  默认处理 status 
    ///// </summary>
    ///// <param name="key">过滤项的key</param>
    ///// <param name="value"> 过滤项的值</param>
    ///// <param name="sqlParas">sql语句中的参数</param>
    ///// <returns></returns>
    //protected virtual string BuildSimpleSearch_FilterItemSql(string key, string value, Dictionary<string, object> sqlParas)
    //{
    //    switch (key)
    //    {
    //        case "status":
    //            sqlParas.Add("@status", value.ToInt32());
    //            if (value.EndsWith("9"))
    //                return " t.`status`>@status";

    //            return value.EndsWith("1") ? " t.`status`<@status" : " t.`status`=@status";

    //        case "owner_uid":
    //            sqlParas.Add("@owner_uid", value.ToInt64());
    //            return " t.`owner_uid`=@owner_uid";

    //        case "tenant_id":
    //            sqlParas.Add("@tenant_id", value.ToInt64());
    //            return " t.`tenant_id`=@tenant_id";
    //    }

    //    return string.Empty;
    //}

    ///// <summary>
    /////  构建排序sql
    ///// </summary>
    ///// <param name="orders"></param>
    ///// <returns></returns>
    //protected virtual string BuildSimpleSearch_OrderSql(Dictionary<string, SortType> orders)
    //{
    //    return " Order BY t.`id` DESC";
    //}

    #endregion

    /// <summary>
    ///  根据状态值处理状态的语句
    ///     以 9 结尾，返回:t.`status`&gt;@status
    ///     以 1 结尾，返回:t.`status`&lt;@status
    ///     其他，    返回:t.`status`=@status
    /// </summary>
    /// <param name="statusVal"></param>
    /// <returns></returns>
    protected static string GenerateStatusSql(string statusVal)
    {
        if (statusVal.EndsWith("9"))
            return "t.`status`>@status";

        return statusVal.EndsWith("1") ? "t.`status`<@status" : "t.`status`=@status";
    }

}


