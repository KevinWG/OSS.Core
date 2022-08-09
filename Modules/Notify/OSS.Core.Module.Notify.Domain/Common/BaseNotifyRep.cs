using OSS.Core.Domain;
using OSS.Core.Rep.Mysql;
using OSS.Tools.Config;

namespace OSS.Core.Module.AppCenter
{
    public abstract class BaseNotifyRep<TType> : BaseMysqlRep<TType, long>
        where TType : BaseOwnerMo<long>, new()
    {
        private static readonly string _writeConnection = ConfigHelper.GetConnectionString("WriteConnection");
        private static readonly string _readConnection  = ConfigHelper.GetConnectionString("ReadConnection");
        protected BaseNotifyRep(string tableName) : base(_writeConnection, _readConnection, tableName)
        {
        }
    }
}
