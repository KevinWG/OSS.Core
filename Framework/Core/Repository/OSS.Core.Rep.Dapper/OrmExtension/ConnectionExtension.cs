#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore仓储层 ——  Connection 方法扩展类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-7
*       
*****************************************************************************/

#endregion

using Dapper;
using OSS.Common.Resp;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace OSS.Core.Rep.Dapper
{
    internal static class ConnectionExtension
    {
        #region    插入扩展

        public static Task<int> Insert<TType>(this IDbConnection con, string tableName, TType mo)
        {
            if (mo==null)
            {
                throw new ArgumentNullException(nameof(mo), $"({tableName})表新增实体为空");
            }
            
            if (string.IsNullOrEmpty(tableName))
                tableName = mo.GetType().Name;

            var sql = GetInsertSql<TType>(tableName);

            return con.ExecuteAsync(sql, mo);
        }

        public static Task<int> InsertList<TType>(this IDbConnection con, string tableName, IList<TType> list)
        {
            if (string.IsNullOrEmpty(tableName))
                tableName = typeof(TType).Name;

            var sql = GetInsertSql<TType>(tableName);

            return con.ExecuteAsync(sql, list);
        }


        private static string GetInsertSql<TType>(string tableName)
        {
            //  todo 未来针对类型，添加语句缓存
            var properties = typeof(TType).GetProperties();

            var sqlCols = new StringBuilder("INSERT INTO ");
            sqlCols.Append(tableName).Append(" (");

            var sqlValues = new StringBuilder(" VALUES (");
            var isStart = false;

            foreach (var propertyInfo in properties)
            {
                //if (haveAuto)
                //{
                //    var isAuto = propertyInfo.GetCustomAttribute<AutoColumnAttribute>() != null;
                //    if (isAuto)
                //    {
                //        continue;
                //    }
                //}

                if (isStart)
                {
                    sqlCols.Append(",");
                    sqlValues.Append(",");
                }
                else
                    isStart = true;
                sqlCols.Append("`").Append(propertyInfo.Name).Append("`");
                sqlValues.Append("@").Append(propertyInfo.Name);
            }
            sqlCols.Append(")");
            sqlValues.Append(")");
            sqlCols.Append(sqlValues);

            //if (haveAuto)
            //    sqlCols.Append(";SELECT LAST_INSERT_ID();");
            return sqlCols.ToString();
        }
        #endregion

        public static async Task<Resp> UpdatePartial<TType>(this IDbConnection con, string tableName,
            Expression<Func<TType, object>> update, Expression<Func<TType, bool>> where, object? mo)
        {
            if (string.IsNullOrEmpty(tableName))
                tableName = typeof(TType).Name;

            var visitor = new SqlExpressionVisitor();

            var updateSql = GetVisitExpressSql(visitor, update, SqlVistorType.Update);
            var whereSql = GetVisitExpressSql(visitor, where, SqlVistorType.Where);
            var sql = string.Concat("UPDATE ", tableName, " SET ", updateSql, whereSql);

            var paras = GetExecuteParas(mo, visitor);
            var row = await con.ExecuteAsync(sql, paras);

            return row > 0 ? new Resp() : new Resp().WithResp(RespCodes.OperateFailed, "更新失败!");
        }

        /// <summary>
        ///  获取单项扩展
        /// </summary>
        /// <typeparam name="PType"></typeparam>
        /// <typeparam name="RType"></typeparam>
        /// <param name="con"></param>
        /// <param name="whereExp"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static async Task<RType> Get<PType, RType>(this IDbConnection con, string tableName,
            Expression<Func<PType, bool>> whereExp)
        {
            if (string.IsNullOrEmpty(tableName))
                tableName = typeof(PType).Name;

            var sqlVisitor = new SqlExpressionVisitor();
            var whereSql   = GetVisitExpressSql(sqlVisitor, whereExp, SqlVistorType.Where);

            var sqlStr = string.Concat("SELECT * FROM ", tableName, whereSql);
            var paras  = GetExecuteParas(null, sqlVisitor);

            return await con.QuerySingleOrDefaultAsync<RType>(sqlStr, paras);
        }
        
        public static async Task<List<RType>> GetList<PType, RType>(this IDbConnection con, string tableName,
                                                                    Expression<Func<PType, bool>> whereExp)
            //where PType : class
        {
            if (string.IsNullOrEmpty(tableName))
                tableName = typeof(PType).Name;

            var sqlVisitor = new SqlExpressionVisitor();
            var whereSql   = GetVisitExpressSql(sqlVisitor, whereExp, SqlVistorType.Where);

            var sqlStr = string.Concat("SELECT * FROM ", tableName, whereSql);
            var paras  = GetExecuteParas(null, sqlVisitor);

            return (await con.QueryAsync<RType>(sqlStr, paras)).ToList();
        }

        /// <summary>
        ///   处理where条件表达式，如果表达式为空，默认使用Id
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="exp"></param>
        /// <param name="visType"></param>
        private static string GetVisitExpressSql(SqlExpressionVisitor visitor, Expression exp, SqlVistorType visType)
        {
            if (visType == SqlVistorType.Update)
            {
                var updateFlag = new SqlVistorFlag(SqlVistorType.Update);
                visitor.Visit(exp, updateFlag);
                return updateFlag.sql;
            }

            var whereFlag = new SqlVistorFlag(SqlVistorType.Where);
            visitor.Visit(exp, whereFlag);

            var sql = string.Concat(" WHERE ", whereFlag.sql);

            return sql;
        }

        private static object? GetExecuteParas(object? mo, SqlExpressionVisitor visitor)
        {
            if (!visitor.parameters.Any())
                return mo;

            var paras = new DynamicParameters(visitor.parameters);
            if (mo == null || !visitor.properties.Any())
                return paras;

            paras.AddDynamicParams(mo);
            return paras;
        }
    }
}

