using Microsoft.Data.SqlClient;
using System.Data;

namespace OSS.Core.Rep.Dapper
{
    /// <summary>
    ///  sqlserver基础仓储类
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    /// <typeparam name="IdType"></typeparam>
    public class BaseSqlServerRep<TType, IdType> : BaseRep<TType, IdType>
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
}
