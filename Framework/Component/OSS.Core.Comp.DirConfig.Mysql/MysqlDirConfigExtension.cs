using Microsoft.Extensions.DependencyInjection;
using OSS.Tools.Config;
using OSS.Tools.DirConfig;

namespace OSS.Core.Comp.DirConfig.Mysql;

public static class MysqlDirConfigCompExtension
{
    private static DirConfigMysqlTool? _dirConfig;

    /// <summary>
    /// 使用字典组件（Mysql存储）
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="opt"></param>
    public static void UserMysqlDirConfigTool(this IServiceCollection serviceCollection, ConnectionOption? opt = null)
    {
        _dirConfig ??= new DirConfigMysqlTool(opt ?? GetDefaultOption());

        DirConfigHelper.ToolProvider = (s) => _dirConfig;
    }


    private static ListConfigMysqlTool? _listConfig = null;
    /// <summary>
    /// 使用列表配置项组件（Mysql存储）
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="opt"></param>
    public static void UserMysqlListConfigTool(this IServiceCollection serviceCollection, ConnectionOption? opt = null)
    {
        _listConfig ??= new ListConfigMysqlTool(opt ?? GetDefaultOption());

        ListConfigHelper.ToolProvider = (s) => _listConfig;
    }

    private static ConnectionOption GetDefaultOption()
    {
        return new ConnectionOption()
        {
            TableName       = "sys_dir_config",
            ReadConnection  = ConfigHelper.GetConnectionString("ReadConnection"),
            WriteConnection = ConfigHelper.GetConnectionString("WriteConnection"),
        };
    }
}

public class ConnectionOption
{
    /// <summary>
    ///  写连接串
    /// </summary>
    public string WriteConnection { get; set; } = "WriteConnection";

    /// <summary>
    ///  读连接串
    /// </summary>
    public string ReadConnection { get; set; } = "ReadConnection";

    /// <summary>
    ///  配置表名
    /// </summary>
    public string TableName { get; set; } = "sys_dir_config";

}


