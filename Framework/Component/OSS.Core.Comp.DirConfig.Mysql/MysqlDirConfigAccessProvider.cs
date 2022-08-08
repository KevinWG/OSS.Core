using System.Data;
using OSS.Common;
using OSS.Core.Comp.DirConfig.Mysql;

namespace OSS.Core.Comp.DirConfig;

public class MysqlDirConfigAccessProvider<T> : IAccessProvider<T>
    where T : IAccess
{
    private readonly string _configKey;
    private readonly string _sourceName;

    /// <summary>
    ///  键值配置的访问信息配置提供者
    /// </summary>
    /// <param name="configKey"></param>
    /// <param name="sourceName"></param>
    public MysqlDirConfigAccessProvider(string configKey, string sourceName = "")
    {
        _configKey  = configKey;
        _sourceName = sourceName;
    }
    
    private readonly DirConfigMysqlTool _dirConfigTool = SingleInstance<DirConfigMysqlTool>.Instance;

    /// <inheritdoc />
    public async Task<T> Get()
    {
        var access = await _dirConfigTool.GetDirConfig<T>(_configKey, _sourceName);
        if (access==null)
            throw new NoNullAllowedException($"未能发现{_configKey} 对应的访问配置信息!");
        
        return access;
    }
}