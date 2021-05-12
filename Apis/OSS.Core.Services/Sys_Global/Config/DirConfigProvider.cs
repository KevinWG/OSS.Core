using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;

using OSS.Tools.DirConfig;
using Newtonsoft.Json;
using OSS.Core.Infrastructure.BasicMos;
using OSS.Core.RepDapper.Sys_Clobal.DirConfig;
using OSS.Core.RepDapper.Sys_Clobal.DirConfig.Mos;

namespace OSS.Core.Services.Sys_Global
{
    public class DirConfigProvider:IToolDirConfig
    {
        #region 实现IToolDirConfig 方法部分

        public async Task<bool> SetDirConfig<TConfig>(string key, TConfig dirConfig) where TConfig : class, new()
        {
            //var setRes=await InsContainer<IDirConfigServiceProxy>.Instance.SetDirConfig(key, dirConfig);
            var confJson = JsonConvert.SerializeObject(dirConfig);
            var updateRes =await DirConfigRep.Instance.Update(key, confJson);
            if (updateRes.IsSuccess())
                return true;

            var getRes =await DirConfigRep.Instance.GetByKey(key);
            if (getRes.IsRespType(RespTypes.ObjectNull))
            {
                var mo = new DirConfigMo();
                mo.dir_key = key;
                mo.dir_vals = confJson;

                mo.InitialBaseFromContext();
                var addRes=await DirConfigRep.Instance.Add(mo);
                return addRes.IsSuccess();
            }
            return false;
        }

        public Task RemoveDirConfig(string key)
        {
            throw new RespException(RespTypes.OperateFailed, "系统配置不得移除！");
        }

        public async Task<TConfig> GetDirConfig<TConfig>(string key) where TConfig : class, new()
        {
            var configRes = await DirConfigRep.Instance.GetByKey(key);
            if (!configRes.IsSuccess())
                return null;

            var jsonStr = configRes.data.dir_vals;
            if (string.IsNullOrEmpty(jsonStr))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<TConfig>(jsonStr);
        }

        #endregion
    }
}
