using Microsoft.Extensions.DependencyInjection;
using OSS.Tools.DirConfig;

namespace OSS.Core.Comp.DirConfig.Mysql;

public static class MysqlDirConfigCompExtension 
{
    /// <summary>
    /// 使用字典组件（Mysql存储）
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="opt"></param>
    public static void UserMysqlDirConfigTool(this IServiceCollection serviceCollection, ConnectionOption? opt=null)
    {
        DirConfigRep.Option            = opt?? _defaultOption;
        DirConfigHelper.DefaultDirTool = new DirConfigMysqlTool();
    }
    private static readonly ConnectionOption _defaultOption = new();
}

public class ConnectionOption
{
    public string WriteConnectionName { get; set; } = "WriteConnection";
    public string ReadConnectionName { get; set; } = "ReadConnection";

    public string TableName { get; set; } = "sys_dir_config";
}


