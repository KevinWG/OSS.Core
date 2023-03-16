using OSS.Core.Domain;
using System.Data;
using Microsoft.Data.SqlClient;

namespace OSS.Core.Rep.Dapper
{
    public class BaseSqlServerRep<TType, IdType> : BaseRep<TType, IdType>
        where TType : BaseMo<IdType>
    {
        private readonly string _writeConnection;
        private readonly string _readConnection;

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="writableConnection">读写分离的写数据库连接串</param>
        /// <param name="readableConnection">读写分离的读数据库连接串</param>
        /// <param name="tableName">当前仓储表名（通用操作使用）</param>
        protected BaseSqlServerRep(string writableConnection, string readableConnection, string tableName) : base(tableName)
        {
            _writeConnection = writableConnection;
            _readConnection  = readableConnection;
        }

        /// <inheritdoc />
        protected override IDbConnection GetDbConnection(bool isWriteOperate)
        {
            return new SqlConnection(isWriteOperate ? _writeConnection : _readConnection);
        }




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
            source = source.Replace("drop ", "");
            source = source.Replace("delete ", "");
            source = source.Replace("update ", "");
            source = source.Replace("insert ", "");
            source = source.Replace("'", "''");
            source = source.Replace(";", "；");
            source = source.Replace("(", "（");
            source = source.Replace(")", "）");
            source = source.Replace("Exec ", "");
            source = source.Replace("Execute ", "");
            source = source.Replace("xp_", "x p_");
            source = source.Replace("sp_", "s p_");
            source = source.Replace("0x", "0 x");
            return source;
        }

        #endregion
    }
}
