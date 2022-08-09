using OSS.Core.Domain;
using OSS.Core.Rep.Mysql;
using OSS.Tools.Config;

namespace OSS.Core.Module.Portal
{
    public abstract class BasePortalRep<TType> : BaseMysqlRep<TType, long>
        where TType : BaseOwnerMo<long>, new()
    {
        protected BasePortalRep(string tableName) : base(
            ConfigHelper.GetConnectionString("WriteConnection")
            , ConfigHelper.GetConnectionString("ReadConnection")
            , tableName
        )
        {
        }
    }
}
