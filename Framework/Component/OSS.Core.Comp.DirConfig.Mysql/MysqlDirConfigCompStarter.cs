using Microsoft.Extensions.DependencyInjection;
using OSS.Tools.DirConfig;

namespace OSS.Core.Comp.DirConfig.Mysql;

public class MysqlDirConfigCompStarter : AppStarter
{
    public override void Start(IServiceCollection serviceCollection)
    {
        DirConfigHelper.DefaultDirTool = new DirConfigMysqlTool();
    }
}
