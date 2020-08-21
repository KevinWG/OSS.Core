using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Helpers;
using OSS.Core.Services.Plugs.Config.IProxies;
using OSS.Tools.DirConfig;

namespace OSS.Core.Services.Sys_Global.ToolProviders
{
    public class DirConfigProvider:IToolDirConfig
    {
        #region 实现IToolDirConfig 方法部分

        public async Task<bool> SetDirConfig<TConfig>(string key, TConfig dirConfig) where TConfig : class, new()
        {
           var setRes=await InsContainer<IDirConfigServiceProxy>.Instance.SetDirConfig(key, dirConfig);
           return setRes.IsSuccess();
        }

        public Task RemoveDirConfig(string key)
        {
            return InsContainer<IDirConfigServiceProxy>.Instance.RemoveDirConfig(key);
        }

        public async Task<TConfig> GetDirConfig<TConfig>(string key) where TConfig : class, new()
        {
            var configRes = await InsContainer<IDirConfigServiceProxy>.Instance.GetConfig<TConfig>(key);
            if (configRes.IsSuccess())
            {
                return configRes.data;
            }

            return null;
        }

        #endregion
    }
}
