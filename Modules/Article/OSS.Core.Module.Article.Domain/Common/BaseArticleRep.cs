using OSS.Core.Domain;
using OSS.Core.Rep.Mysql;
using OSS.Tools.Config;

namespace OSS.Core.Module.Article;

/// <summary>
///  Article 模块仓储基类
/// </summary>
public abstract class BaseArticleRep<MType,IdType> : BaseMysqlRep<MType, IdType>
    where MType : BaseMo<IdType>
{
    private static readonly string _writeConnection = ConfigHelper.GetConnectionString("WriteConnection");
    private static readonly string _readConnection  = ConfigHelper.GetConnectionString("ReadConnection");

    protected BaseArticleRep(string tableName) : base(_writeConnection , _readConnection , tableName)
    {
    }
}
