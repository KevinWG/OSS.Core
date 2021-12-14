using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Enums;
using OSS.Common.Resp;
using OSS.Tools.Config;
using System.Data;
using OSS.Core.ORM.Dapper;
using OSS.Common.Extension;

namespace OSS.Core.RepDapper
{
    public abstract class BaseRep<TRep, TType> : BaseRep<TRep, TType,long>
        where TRep : class, new()
        where TType : BaseOwnerMo, new()
    {
        private static readonly string _writeConnection = ConfigHelper.GetConnectionString("WriteConnection");
        private static readonly string _readConnection = ConfigHelper.GetConnectionString("ReadConnection");

        protected override IDbConnection GetDbConnection(bool isWriteOperate)
        {
            return new MySqlConnection(isWriteOperate ? _writeConnection : _readConnection);
        }

        protected override Task<Resp<RType>> GetById<RType>(long id)
        {
            var sql = string.Concat("select * from ", TableName, " WHERE id=@id and status>@status");

            var dirPara = new Dictionary<string, object>
            {
                { "@id", id }, { "@status", (int)CommonStatus.Deleted }
            };

            return Get<RType>(sql, dirPara);
        }

        #region 简单搜索封装

        protected Task<PageListResp<TType>> SimpleSearch(SearchReq req)
        {
            return SimpleSearch<TType>(req);
        }

        protected Task<PageListResp<RType>> SimpleSearch<RType>(SearchReq req)
        {
            var offCount = req.GetStartRow();
            var whereSql = BuildSimpleSearchWhereSql(req.filter, out var sqlParas);

            var orderSql = BuildSimpleSearchOrderSql(req.orders);

            var totalSql = string.Concat("select count(1) from ", BuildSimpleSearchTableName(req), whereSql);
            var selectSql = string.Concat("select ", BuildSimpleSearchSelectColumns(req)
                , " from ", BuildSimpleSearchTableName(req), " ", whereSql
                , " ", orderSql
                , " limit ", req.size, " offset ", offCount);

            return GetPageList<RType>(selectSql, sqlParas, totalSql);
        }

        protected virtual string BuildSimpleSearchSelectColumns(SearchReq req)
        {
            return $"t.*";
        }
        protected virtual string BuildSimpleSearchTableName(SearchReq req)
        {
            return $"{TableName} t ";
        }

        /// <summary>
        /// 生成 where 语句
        /// </summary>
        /// <param name="searchFilters">搜索的条件信息</param>
        /// <param name="sqlParas">sql语句中的参数信息</param>
        /// <returns></returns>
        protected string BuildSimpleSearchWhereSql(Dictionary<string, string> searchFilters, out Dictionary<string, object> sqlParas)
        {
            if (!searchFilters.ContainsKey("status"))
            {
                searchFilters.Add("status", "-999");
            }

            var haveValue = false;
            var strBuilder = new StringBuilder();
            sqlParas = new Dictionary<string, object>(searchFilters.Count);

            foreach (var filter in searchFilters)
            {
                if (string.IsNullOrEmpty(filter.Value?.ToString()))
                {
                    continue;
                }
                var itemSql = BuildSimpleSearchWhereSqlByFilterItem(filter.Key, filter.Value, sqlParas);
                if (string.IsNullOrEmpty(itemSql))
                    continue;

                if (haveValue)
                {
                    strBuilder.Append(" and ");
                }
                else
                {
                    haveValue = true;
                }
                strBuilder.Append(itemSql);
            }

            return strBuilder.Length > 0 ? string.Concat(" where ", strBuilder) : string.Empty;
        }




        /// <summary>
        /// 生成 where 条件子语句 -- 针对每个单独的 filter item
        ///  默认处理 status 
        /// </summary>
        /// <param name="key">过滤项的key</param>
        /// <param name="value"> 过滤项的值</param>
        /// <param name="sqlParas">sql语句中的参数</param>
        /// <returns></returns>
        protected virtual string BuildSimpleSearchWhereSqlByFilterItem(string key, string value, Dictionary<string, object> sqlParas)
        {
            switch (key)
            {
                case "status":
                    sqlParas.Add("@status", value.ToInt32());
                    return value.ToString() == "-999" ? " t.`status`>@status" : " t.`status`=@status";
            }
            return string.Empty;
        }

        protected virtual string BuildSimpleSearchOrderSql(Dictionary<string, SortType> orders)
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
