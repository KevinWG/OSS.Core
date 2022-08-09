﻿
using System.Data;
using System.Text;

using MySql.Data.MySqlClient;

using OSS.Common;
using OSS.Common.Resp;
using OSS.Common.Extension;

using OSS.Core.Domain;
using OSS.Core.Rep.Dapper;

namespace OSS.Core.Rep.Mysql
{
    [Obsolete]
    public abstract class BaseSingleMysqlRep<TRep, TType, IdType> : BaseSingleRep<TRep,TType, IdType>
        where TRep : class, new()
        where TType :  BaseMo<IdType>, new()
    {
        private readonly string _writeConnection ;
        private readonly string _readConnection ;

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="writableConnection">读写分离的写数据库连接串</param>
        /// <param name="readableConnection">读写分离的读数据库连接串</param>
        protected BaseSingleMysqlRep(string writableConnection,string readableConnection)
        {
            _writeConnection = writableConnection;
            _readConnection = readableConnection ;
        }

        /// <inheritdoc />
        protected override IDbConnection GetDbConnection(bool isWriteOperate)
        {
            return new MySqlConnection(isWriteOperate ? _writeConnection : _readConnection);
        }

        /// <inheritdoc />
        public override Task<Resp<RType>> GetById<RType>(IdType id)
        {
            var sql = string.Concat("select * from ", TableName, " WHERE id=@id and status>@status");

            var dirPara = new Dictionary<string, object>
            {
                { "@id", id }, { "@status", (int)CommonStatus.Deleted }
            };

            return Get<RType>(sql, dirPara);
        }



        #region 简单搜索封装

        /// <summary>
        ///  通用搜索
        /// </summary>
        /// <returns></returns>
        protected Task<PageListResp<TType>> SimpleSearch(SearchReq req)
        {
            return SimpleSearch<TType>(req);
        }

        /// <summary>
        ///  通用搜索
        /// </summary>
        /// <returns></returns>
        protected Task<PageListResp<RType>> SimpleSearch<RType>(SearchReq req)
        {
            var sqlParas = new Dictionary<string, object>(req.filter.Count);

            var columns = BuildSimpleSearch_SelectColumns(req);
            var tableName = BuildSimpleSearch_TableName(req, sqlParas);
            var whereSql = BuildSimpleSearch_WhereSql(req.filter, sqlParas);
            var orderSql = BuildSimpleSearch_OrderSql(req.orders);

            var offCount = req.GetStartRow();
            var totalSql = req.req_count ? $"select count(1) from {tableName} {whereSql}" : string.Empty;
            var selectSql =
                $"select {columns} from {tableName} {whereSql} {orderSql} limit {req.size} offset {offCount}";

            return GetPageList<RType>(selectSql, sqlParas, totalSql);
        }


        /// <summary>
        ///  构建通用搜索的列名
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        protected virtual string BuildSimpleSearch_SelectColumns(SearchReq req)
        {
            return $"t.*";
        }

        /// <summary>
        ///  构建通用搜索的表名
        /// </summary>
        /// <param name="req"></param>
        /// <param name="sqlParas"></param>
        /// <returns></returns>
        protected virtual string BuildSimpleSearch_TableName(SearchReq req, Dictionary<string, object> sqlParas)
        {
            return $"{TableName} t ";
        }

        /// <summary>
        /// 生成 where 语句
        /// </summary>
        /// <param name="searchFilters">搜索的条件信息</param>
        /// <param name="sqlParas">sql语句中的参数信息</param>
        /// <returns></returns>
        protected virtual string BuildSimpleSearch_WhereSql(Dictionary<string, string> searchFilters,
            Dictionary<string, object> sqlParas)
        {
            if (!searchFilters.ContainsKey("status"))
                searchFilters.Add("status", "-9999");

            var strBuilder = new StringBuilder("where 1=1 ");
            foreach (var filter in searchFilters)
            {
                if (string.IsNullOrEmpty(filter.Value?.ToString()))
                {
                    continue;
                }

                // 使用case映射， 防止其他未预料的 key 值，或者外部攻击的字段脚本
                var itemSql = BuildSimpleSearch_FilterItemSql(filter.Key, filter.Value, sqlParas);
                if (string.IsNullOrEmpty(itemSql))
                    continue;

                strBuilder.Append(" and ");
                strBuilder.Append(itemSql);
            }

            return strBuilder.ToString();
        }


        /// <summary>
        /// 生成 where 条件子语句 -- 针对每个单独的 filter item
        ///  默认处理 status 
        /// </summary>
        /// <param name="key">过滤项的key</param>
        /// <param name="value"> 过滤项的值</param>
        /// <param name="sqlParas">sql语句中的参数</param>
        /// <returns></returns>
        protected virtual string BuildSimpleSearch_FilterItemSql(string key, string value,
            Dictionary<string, object> sqlParas)
        {
            switch (key)
            {
                case "status":
                    sqlParas.Add("@status", value.ToInt32());
                    if (value.EndsWith("9"))
                    {
                        return " t.`status`>@status";
                    }
                    else if (value.EndsWith("1"))
                    {
                        return " t.`status`<@status";
                    }
                    return " t.`status`=@status";
                case "owner_uid":
                    sqlParas.Add("@owner_uid", value.ToInt64());
                    return " t.`owner_uid`=@owner_uid";
            }

            return string.Empty;
        }

        /// <summary>
        ///  构建排序sql
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        protected virtual string BuildSimpleSearch_OrderSql(Dictionary<string, SortType> orders)
        {
            return " Order BY `id` DESC";
        }

        #endregion


        #region 辅助方法

        /// <summary>
        /// 过滤 Sql 语句字符串中的注入脚本
        /// </summary>
        /// <param name="source">传入的字符串</param>
        /// <returns>过滤后的字符串</returns>
        protected static string SqlFilter(string source)
        {
            source = source.Replace("\"", "");
            source = source.Replace("&", "&amp");
            source = source.Replace("<", "&lt");
            source = source.Replace(">", "&gt");
            source = source.Replace("%", "");
            source = source.Replace("drop", "");
            source = source.Replace("delete", "");
            source = source.Replace("update", "");
            source = source.Replace("insert", "");
            source = source.Replace("'", "''");
            source = source.Replace(";", "；");
            source = source.Replace("(", "（");
            source = source.Replace(")", "）");
            source = source.Replace("Exec", "");
            source = source.Replace("Execute", "");
            source = source.Replace("xp_", "x p_");
            source = source.Replace("sp_", "s p_");
            source = source.Replace("0x", "0 x");
            return source;
        }

        #endregion

    }

}