using OSS.Core.Domain;
using OSS.Core.Rep.Mysql;
using OSS.Tools.Config;

namespace TM.WMS;

/// <summary>
///  仓储管理 模块仓储基类
/// </summary>
public abstract class BaseWMSRep<MType,IdType> : BaseMysqlRep<MType, IdType>
    where MType : BaseMo<IdType>
{
    private static readonly string _writeConnection = ConfigHelper.GetConnectionString("WriteConnection");
    private static readonly string _readConnection  = ConfigHelper.GetConnectionString("ReadConnection");

    protected BaseWMSRep(string tableName) : base(_writeConnection , _readConnection , tableName)
    {
    }
}
