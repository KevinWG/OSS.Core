using OSS.Common.Resp;
using OSS.Core.Rep.Mysql;
using OSS.Tools.Cache;

namespace OSS.Core.Comp.DirConfig.Mysql;

internal class DirConfigRep : BaseMysqlRep<DirConfigMo, string>
{
    public DirConfigRep(ConnectionOption option) : base(option.WriteConnection, option.ReadConnection, option.TableName)
    {

    }

    private const string System_Dir_Config_ByKey = "System_Dir_Config_";

    public Task<IResp> UpdateVal(string key, string confJson)
    {
        var cacheKey = string.Concat(System_Dir_Config_ByKey, key);
        return Update(u => new {u.config_val}, w => w.id == key, new {config_val = confJson})
            .WithRespCacheClearAsync(cacheKey);
    }

    public override Task<IResp<DirConfigMo>> GetById(string key)
    {
        var cacheKey = string.Concat(System_Dir_Config_ByKey, key);

        var getfunc = () => base.GetById(key);

        return getfunc.WithRespCacheAsync(cacheKey, TimeSpan.FromHours(1));
    }
}