using System.Collections.Concurrent;
using System.Data;
using System.Reflection;
using System.Text;
using Dapper;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;

namespace OSS.Core.RepDapper.OrmExtention
{
    public static class ConnoctionExtention
    {
        /// <summary>
        ///   插入新记录
        /// </summary>
        /// <param name="con"></param>
        /// <param name="mo"></param>
        /// <param name="isIdAuto"> 【Id】主键是否是自增长，如果是同步返回，不是则需要外部传值</param>
        /// <param name="tableName">如果为空则以TType.GetType().Name</param>
        /// <returns></returns>
        public static ResultIdMo Insert<TType>(this IDbConnection con, TType mo, bool isIdAuto = true,
            string tableName = null)
            where TType : new()
        {
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = mo.GetType().Name;
            }
            
            //  1.  生成语句
            StringBuilder sqlCols = new StringBuilder("INSERT INTO ");
            sqlCols.Append(tableName).Append(" (");

            StringBuilder sqlValues = new StringBuilder(" VALUES (");
            var properties = typeof(TType).GetProperties();

            bool isStart = false;
            foreach (var propertyInfo in properties)
            {
                if (isIdAuto && propertyInfo.Name == "Id")
                    continue;
                if (isStart)
                {
                    sqlCols.Append(",");
                    sqlValues.Append(",");
                }
                else
                    isStart = true;
                sqlCols.Append(propertyInfo.Name);
                sqlValues.Append("@").Append(propertyInfo.Name);
            }
            sqlCols.Append(")");
            sqlValues.Append(")");
            
            sqlCols.Append(sqlValues);
            if (isIdAuto)
                sqlCols.Append(";SELECT LAST_INSERT_ID();");


            // 2. 处理参数
            var para = new DynamicParameters(mo);

            var id = isIdAuto ? con.ExecuteScalar<long>(sqlCols.ToString(), para) : con.Execute(sqlCols.ToString(), para);
            return id > 0 ? new ResultIdMo(isIdAuto ? id : 0) : new ResultIdMo(ResultTypes.AddFail, "添加操作失败！");
        }

        // 更新
        //public virtual int UpdateOnly(Expression<Func<TType, object>> updateCols, Expression<Func<TType, bool>> whereCondition, Func<string, Dictionary<string, object>> additonnalFunc)
        //{
        //    return 0;
        //}

        //public int UpdateAllById(TType mo)
        //{
        //    return 0;
        //}

        ////  根据指定列删除
        //public int Delete()
        //{
        //    //  软删除，底层不提供物理删除方法
        //    return 0;
        //}

        ////// 根据指定列查询
        //public TType Get()
        //{
        //    return new TType();
        //}

        //public virtual PageListMo<TType> GetPageList(SearchMo mo)
        //{
        //    return new PageListMo<TType>();
        //}

    }



    internal static class OrmCacheUtil
    {
        private static readonly ConcurrentDictionary<string, OrmCacheInfo> cacheList;

        static OrmCacheUtil()
        {
            cacheList = new ConcurrentDictionary<string, OrmCacheInfo>();
        }


        public static OrmCacheInfo GetCacheInfo(string key)
        {
            return cacheList.TryGetValue(key,out OrmCacheInfo info) ? info : null;
        }

        public static void AddCacheInfo(string key, OrmCacheInfo info)
        {

        }
    }

    public class OrmCacheInfo
    {
        public string Sql { get; set; }

        //public Func<>
    }
}
