#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore仓储层 ——  Connection 方法扩展类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-7
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention.DTO;

namespace OSS.Core.RepDapper.OrmExtention
{
    internal static class ConnoctionExtention 
    {
        #region    插入扩展
        /// <summary>
        ///   插入新记录
        /// </summary>
        /// <param name="con">数据库连接</param>
        /// <param name="mo">对应实体</param>
        /// <param name="isIdAuto"> 【Id】主键是否是自增长，如果是同步返回，不是则需要外部传值（不通过Attribute处理是为了尽可能减少依赖 </param>
        /// <param name="tableName">如果为空则以TType.GetType().Name</param>
        /// <returns></returns>
        public static async Task<ResultIdMo> Insert<TType>(this IDbConnection con, TType mo,string tableName = null, bool isIdAuto = true)
        {
            if (string.IsNullOrEmpty(tableName))
                tableName = mo.GetType().Name;

            var key = string.Concat(tableName, "|", isIdAuto, "|", con.ConnectionString, "|", typeof(TType).FullName);
            var ormInfo = GetInsertOrmCacheInfo<TType>(tableName, key);

            var para = new DynamicParameters(ormInfo.ParaFunc?.Invoke(mo));

            long id;
            if (isIdAuto)
                id = await con.ExecuteScalarAsync<long>(ormInfo.Sql, para);
            else
                id = await con.ExecuteAsync(ormInfo.Sql, para);

            return id > 0 ? new ResultIdMo(id) : new ResultIdMo(ResultTypes.AddFail, "添加操作失败！");
        }

        private static OrmOperateInfo GetInsertOrmCacheInfo<TType>(string tableName, string key) 
        {
            var cache = OrmCacheUtil.GetCacheInfo(key);
            if (cache != null) return cache;

            cache = new OrmOperateInfo
            {
                Sql = GetInserSql<TType>(tableName, out List<PropertyInfo> prolist),
                ParaFunc = SqlParameterEmit.CreateDicDeleMothed<TType>(prolist)
            };
            OrmCacheUtil.AddCacheInfo(key, cache);
            return cache;
        }

        private static string GetInserSql<TType>(string tableName, out List<PropertyInfo> proList)
        {
            //  1.  生成语句
            var sqlCols = new StringBuilder("INSERT INTO ");
            sqlCols.Append(tableName).Append(" (");

            var sqlValues = new StringBuilder(" VALUES (");
            var properties = typeof(TType).GetProperties();
            proList = new List<PropertyInfo>(properties.Length);

            bool isStart = false, haveAuto = false;
            foreach (var propertyInfo in properties)
            {
                var isAuto = propertyInfo.GetCustomAttribute<AutoColumnAttribute>() != null;
                if (isAuto)
                {
                    haveAuto = true;
                    continue;
                }
                if (isStart)
                {
                    sqlCols.Append(",");
                    sqlValues.Append(",");
                }
                else
                    isStart = true;
                sqlCols.Append("`").Append(propertyInfo.Name).Append("`");
                sqlValues.Append("@").Append(propertyInfo.Name);
                proList.Add(propertyInfo);
            }
            sqlCols.Append(")");
            sqlValues.Append(")");
            sqlCols.Append(sqlValues);

            if (haveAuto) sqlCols.Append(";SELECT LAST_INSERT_ID();");
            return sqlCols.ToString();
        }
        #endregion
        
        #region  全量更新

        /// <summary>
        ///  全量更新
        /// </summary>
        /// <typeparam name="TType">更新的实体类型</typeparam>
        /// <param name="mo">需要更新的实体</param>
        /// <param name="where">更新条件，为空则默认使用Id</param>
        /// <returns></returns>
        internal static async Task<ResultMo> UpdateAll<TType>(this IDbConnection con, TType mo,
            Expression<Func<TType, bool>> where = null, string tableName = null)
        {
            if (string.IsNullOrEmpty(tableName))
                tableName = typeof(TType).Name;

            var visitor = new SqlExpressionVisitor();
            var sqlStr = new StringBuilder("UPDATE ");

            var whereSql = VisitWhereExpress(visitor, where); //  放在前面获取where后的成员参数，update时排除
            GetUpdateAllSql<TType>(tableName, sqlStr, visitor);
            sqlStr.Append(whereSql); //  where语句追加在后边

            return await ExecuteUpdate(con, mo, tableName, sqlStr, visitor);
        }

        private static void GetUpdateAllSql<TType>(string tableName, StringBuilder sqlStr, SqlExpressionVisitor visitor)
        {
            sqlStr.Append(tableName).Append(" SET ");
            var pros = typeof(TType).GetProperties();

            var isStart = false;
            foreach (var pro in pros)
            {
                if (visitor.Properties.ContainsKey(pro.Name)
                    || pro.GetCustomAttribute<AutoColumnAttribute>() != null)
                    continue;

                sqlStr.Append(isStart ? "," : string.Empty)
                    .Append("`").Append(pro.Name).Append("`").Append("=").Append("@").Append(pro.Name);

                if (!isStart)
                    isStart = true;

                visitor.Properties.Add(pro.Name, pro);
            }
        }

        #endregion

        internal static async Task<ResultMo> UpdatePartail<TType>(this IDbConnection con, TType mo,
            Expression<Func<TType, object>> update, string tableName = null, Expression<Func<TType, bool>> where = null)
        {
            if (string.IsNullOrEmpty(tableName))
                tableName = typeof(TType).Name;

            var sqlBuilder = new StringBuilder();
            var visitor = new SqlExpressionVisitor();
            GetUpdateExpressionSql(update, where, tableName, visitor, sqlBuilder);

            return await ExecuteUpdate(con, mo, tableName, sqlBuilder, visitor);
        }

        private static async Task<ResultMo> ExecuteUpdate<TType>(IDbConnection con, TType mo, string tableName,
            StringBuilder sqlBuilder,
            SqlExpressionVisitor visitor)
        {
            var opeInfo = GetOrmOperateCache<TType>(con.ConnectionString, sqlBuilder.ToString(), tableName,
                visitor.Properties.Select(e => e.Value));
            var paraDics = opeInfo.ParaFunc?.Invoke(mo) ?? new Dictionary<string, object>();

            foreach (var p in visitor.Parameters)
                paraDics.Add(p.Key, p.Value);

            var row =await con.ExecuteAsync(opeInfo.Sql, new DynamicParameters(paraDics));
            return row > 0 ? new ResultMo() : new ResultMo(ResultTypes.UpdateFail, "更新失败");
        }

        private static OrmOperateInfo GetOrmOperateCache<TType>(string conStr, string sql, string tableName,
            IEnumerable<PropertyInfo> prolist)
        {
            if (!prolist.Any())
                return new OrmOperateInfo() {Sql = sql, ParaFunc = null};

            string key = $"{tableName}|{sql}|{conStr}";
            var cacheInfo = OrmCacheUtil.GetCacheInfo(key);
            if (cacheInfo != null)
                return cacheInfo;
            cacheInfo = new OrmOperateInfo
            {
                Sql = sql,
                ParaFunc = SqlParameterEmit.CreateDicDeleMothed<TType>(prolist)
            };
            OrmCacheUtil.AddCacheInfo(key, cacheInfo);

            return cacheInfo;
        }

        private static void GetUpdateExpressionSql<TType>(Expression<Func<TType, object>> update,
            Expression<Func<TType, bool>> where, string tableName,
            SqlExpressionVisitor visitor, StringBuilder sql)
        {
            sql.Append("UPDATE ").Append(tableName).Append(" SET ");

            var updateFlag = new SqlVistorFlag(SqlVistorType.Update);
            visitor.Visit(update, updateFlag);

            sql.Append(updateFlag.Sql);
            sql.Append(" ").Append(VisitWhereExpress(visitor, where));
        }

        /// <summary>
        ///   处理where条件表达式，如果表达式为空，默认使用Id
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="visitor"></param>
        /// <param name="where"></param>
        private static string VisitWhereExpress<TType>(SqlExpressionVisitor visitor, Expression<Func<TType, bool>> where)
        {
            if (where != null)
            {
                var whereFlag = new SqlVistorFlag(SqlVistorType.Where);
                visitor.Visit(@where, whereFlag);
                return string.Concat(" WHERE ", whereFlag.Sql);
            }

            const string sql = " WHERE Id=@Id";
            if (visitor.Properties.ContainsKey("Id")) return sql;

            var p = typeof(TType).GetProperty("Id");
            if (p == null)
                throw new Exception("Update操作中where条件为空，且未发现Id属性");
            visitor.Properties.Add("Id", p);
            return sql;
        }

        /// <summary>
        ///  获取单项扩展
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="con"></param>
        /// <param name="mo"></param>
        /// <param name="whereExp"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static async Task<TType> Get<TType>(this IDbConnection con, string tableName=null, TType mo = default(TType), Expression<Func<TType, bool>> whereExp = null)
        {
            if (string.IsNullOrEmpty(tableName))
                tableName = typeof(TType).Name;

            var sqlVisitor=new SqlExpressionVisitor();
            var whereSql = VisitWhereExpress(sqlVisitor, whereExp);

            var sqlStr = string.Concat("SELECT * FROM ", tableName, whereSql);

            var opeInfo = GetOrmOperateCache<TType>(con.ConnectionString, sqlStr, tableName,sqlVisitor.Properties.Select(e => e.Value));
            var paraDics = opeInfo.ParaFunc?.Invoke(mo) ?? new Dictionary<string, object>();

            foreach (var p in sqlVisitor.Parameters)
                paraDics.Add(p.Key, p.Value);

            return await con.QuerySingleAsync<TType>(opeInfo.Sql, paraDics);
        }

    }



    internal static class OrmCacheUtil
    {
        private static readonly ConcurrentDictionary<string, OrmOperateInfo> cacheList;

        static OrmCacheUtil()
        {
            cacheList = new ConcurrentDictionary<string, OrmOperateInfo>();
        }


        public static OrmOperateInfo GetCacheInfo(string key)
        {
            return cacheList.TryGetValue(key, out OrmOperateInfo info) ? info : null;
        }

        public static void AddCacheInfo(string key, OrmOperateInfo info)
        {
            cacheList.TryAdd(key, info);
        }
    }

    public class OrmOperateInfo
    {
        public string Sql { get; set; }

        public Func<object, Dictionary<string, object>> ParaFunc { get; set; }
    }
}

