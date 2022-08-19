using OSS.Core.Domain;
using OSS.Core.Rep.Mysql;
using OSS.Tools.Config;

namespace OSS.Core.Module.Pipeline;

/// <summary>
///  Pipeline 模块仓储基类
/// </summary>
public abstract class BasePipelineRep<MType,IdType> : BaseMysqlRep<MType, IdType>
    where MType : BaseMo<IdType>
{
    private static readonly string _writeConnection = ConfigHelper.GetConnectionString("WriteConnection");
    private static readonly string _readConnection  = ConfigHelper.GetConnectionString("ReadConnection");

    protected BasePipelineRep(string tableName) : base(_writeConnection , _readConnection , tableName)
    {
    }
}
