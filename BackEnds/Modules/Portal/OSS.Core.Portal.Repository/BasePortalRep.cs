using OSS.Core.Domain;
using OSS.Core.Rep.Mysql;
using OSS.Tools.Config;

namespace OSS.Core.Portal.Repository
{
    public  abstract class BasePortalRep<TRep, TType, IdType> : BaseMysqlRep<TRep, TType, IdType>
        where TRep : class, new()
        where TType : BaseMo<IdType>, new()
    {

        private static readonly string _writeConnection = ConfigHelper.GetConnectionString("WriteConnection");
        private static readonly string _readConnection = ConfigHelper.GetConnectionString("ReadConnection");

        protected BasePortalRep() : base(_writeConnection, _readConnection)
        {
        }
    }
}
