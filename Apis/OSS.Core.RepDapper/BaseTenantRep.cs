using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Enums;
using OSS.Core.Infrastructure.BasicMos;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Mos;
using OSS.Core.ORM.Mysql.Dapper;
using OSS.Tools.Config;

namespace OSS.Core.RepDapper
{
    public abstract class BaseTenantRep<TRep, TType> : BaseMysqlRep<TRep, TType,string>
        where TRep : class, new()
        where TType : BaseOwnerMo, new()
    {
        protected const string defaultOrderSql = " ORDER BY `id` DESC";
        private static readonly string _writeConnection = ConfigHelper.GetConnectionString("WriteConnection");
        private static readonly string _readConnection = ConfigHelper.GetConnectionString("ReadConnection");

        /// <summary>
        ///  租户Id
        /// </summary>
        public string OwnerTId => AppReqContext.Identity.tenant_id;

        protected override MySqlConnection GetDbConnection(bool isWriteOperate)
        {
            return new MySqlConnection(isWriteOperate ? _writeConnection : _readConnection);
        }

        #region 简单搜索封装

        protected Task<PageListResp<TType>> SimpleSearch(SearchReq req)
        {
            return SimpleSearch<TType>(req);
        }

        protected Task<PageListResp<RType>> SimpleSearch<RType>(SearchReq req)
        {
            var offCount = req.GetStartRow();
            var whereSql = BuildSimpleSearchWhereSql(req.filters, out var sqlParas);

            var totalSql = string.Concat("select count(1) from ",BuildSimpleSearchTableName(req), whereSql);
            var selectSql = string.Concat("select ", BuildSimpleSearchSelectColumns(req), " from ", BuildSimpleSearchTableName(req), whereSql, defaultOrderSql, " limit ", req.size, " offset ", offCount);

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
            FillDefaultSearchFilter(searchFilters);

            var haveValue = false;
            var strBuilder = new StringBuilder();
            sqlParas = new Dictionary<string, object>(searchFilters.Count);

            foreach (var filter in searchFilters.Where(filter => !string.IsNullOrEmpty(filter.Value)))
            {
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
        ///  默认处理 status 和 owner_tid 
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
                    sqlParas.Add("@status", value);
                    return value == "-999" ? " t.`status`>@status" : " t.`status`=@status";
                case "owner_tid":
                    sqlParas.Add("@owner_tid", value);
                    return " t.`owner_tid`=@owner_tid";
            }
            return string.Empty;
        }

        private void FillDefaultSearchFilter(Dictionary<string, string> searchFilters)
        {
            if (!searchFilters.ContainsKey("status"))
            {
                searchFilters.Add("status", "-999");
            }

            // 非超级应用必须携带租户信息
            if (AppReqContext.Identity.app_type == AppType.SystemManager)
                return;

            if (!searchFilters.ContainsKey("owner_tid"))
            {
                searchFilters.Add("owner_tid", OwnerTId);
            }
            else
            {
                searchFilters["owner_tid"] = OwnerTId;
            }
        }



        #endregion
        
        #region   基础CRUD SQL操作方法 重写
        
        /// <inheritdoc />
        public override Task<Resp<TType>> GetById(string id)
        {
            var dirPara = new Dictionary<string, object> {{"@id", id}, {"@owner_tid", OwnerTId},{ "@status",(int)CommonStatus.Deleted }}; 
            var sql =string.Concat( "select * from ",TableName," WHERE id=@id and status>@status and `owner_tid`=@owner_tid");

            return Get<TType>(sql, dirPara);
        }

        /// <inheritdoc />
        protected override Task<Resp<RType>> Get<RType>(string getSql, object para)
        {
#if DEBUG
            if (AppReqContext.Identity.app_type > AppType.SystemManager && !getSql.Contains("owner_tid"))
                throw new ArgumentNullException(nameof(getSql), "SQL中需要不能缺少owner_tid！");
#endif
            return base.Get<RType>(getSql, para);
        }

        /// <inheritdoc />
        protected override Task<PageListResp<RType>> GetPageList<RType>(string selectSql,
            object paras, string totalSql = null)
        {
#if DEBUG
            if (AppReqContext.Identity.app_type > AppType.SystemManager && !selectSql.Contains("owner_tid"))
                throw new ArgumentNullException(nameof(selectSql), "SQL中需要不能缺少owner_tid！");
#endif
            return base.GetPageList<RType>(selectSql, paras, totalSql);
        }


        /// <inheritdoc />
        public override Task<Resp> SoftDeleteById(string id)
        {
            var sql = string.Concat("UPDATE ", TableName, " SET status=@status WHERE id=@id  and `owner_tid`=@owner_tid");

            var dirPara = new Dictionary<string, object>
            {
                {"@id", id},
                {"@status", (int) CommonStatus.Deleted},
                {"@owner_tid", OwnerTId}
            };
            return SoftDelete(sql, dirPara);
        }
        
        /// <inheritdoc />
        protected override Task<Resp> SoftDelete(string sql, object paras)
        {
#if DEBUG
            if (AppReqContext.Identity.app_type > AppType.SystemManager && !sql.Contains("owner_tid"))
                throw new ArgumentNullException(nameof(sql), "SQL中需要不能缺少owner_tid！");
#endif
            return base.SoftDelete(sql, paras);
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
