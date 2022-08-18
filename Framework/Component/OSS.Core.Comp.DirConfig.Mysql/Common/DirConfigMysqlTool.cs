using System.Data;
using Newtonsoft.Json;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Tools.DirConfig;

namespace OSS.Core.Comp.DirConfig.Mysql
{
    internal class DirConfigMysqlTool:IToolDirConfig
    {
        private static readonly DirConfigRep _configRep = new();

        public async Task<bool> SetDirConfig<TConfig>(string key, TConfig dirConfig, string sourceName)
        {
            var confJson  = JsonConvert.SerializeObject(dirConfig);
            var updateRes = await _configRep.UpdateVal(key, confJson);
            if (updateRes.IsSuccess())
                return true;

            var getRes = await _configRep.GetById(key);
            if (!getRes.IsRespCode(RespCodes.OperateObjectNull))
                return false;

            var mo = new DirConfigMo
            {
                id         = key,
                config_val = confJson,
                add_time   = DateTime.Now.ToUtcSeconds()
            };

            if (CoreContext.User.IsAuthenticated)
            {
                mo.owner_uid = CoreContext.User.Identity.id.ToInt64();
            }

            await _configRep.Add(mo);
            return true;
        }

        public Task RemoveDirConfig(string key, string sourceName)
        {
            throw new RespException(RespCodes.OperateFailed, "系统配置不得移除！");
        }

        public async Task<TConfig?> GetDirConfig<TConfig>(string key, string sourceName) 
        {
            var configRes = await _configRep.GetById(key);

            if (configRes.IsRespCode(RespCodes.OperateObjectNull))
                return default;
            
            if (!configRes.IsSuccess() || string.IsNullOrEmpty(configRes.data?.config_val))
                throw new NoNullAllowedException($"未能找到{key}对应的配置信息!");

            var res = JsonConvert.DeserializeObject<TConfig>(configRes.data.config_val);
            return res;
        }
    }
}