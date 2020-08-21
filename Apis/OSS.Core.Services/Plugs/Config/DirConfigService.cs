using System;
using System.Threading.Tasks;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Extensions;
using OSS.Core.Services.Plugs.Config.IProxies;
using OSS.Tools.Cache;
using OSS.Tools.DirConfig;
using OSS.Tools.Log;

namespace OSS.Core.Services.Plugs.Config
{
    public class DirConfigService:IDirConfigServiceProxy
    {
        private static readonly DefaultToolDirConfig _xmlFileConfig=new DefaultToolDirConfig();
        
        public async Task<Resp> SetDirConfig<TConfig>(string key, TConfig dirConfig) where TConfig : class, new()
        {
            await _xmlFileConfig.SetDirConfig(key, dirConfig);
            await CacheHelper.RemoveAsync(string.Concat(CacheKeys.Plugs_Config_ByKey, key));
            return new Resp();
        }

        public async Task<Resp> RemoveDirConfig(string key)
        {
            await _xmlFileConfig.RemoveDirConfig(key);
            await CacheHelper.RemoveAsync(string.Concat(CacheKeys.Plugs_Config_ByKey, key));
            return new Resp();
        }

        /// <summary>
        ///   获取配置
        /// </summary>
        /// <typeparam name="TConfig"></typeparam>
        /// <param name="key"></param>
        /// <param name="isFromLogModule"></param>
        /// <returns></returns>
        public Task<Resp<TConfig>> GetConfig<TConfig>(string key, bool isFromLogModule=false) where TConfig : class, new()
        {
            try
            {
                Func<Task<Resp<TConfig>>> getFunc =async () =>
                {
                    var config =await _xmlFileConfig.GetDirConfig<TConfig>(key);
                    if (config != null)
                    {
                        return new Resp<TConfig>(config);
                    }
                    return new Resp<TConfig>().WithResp(RespTypes.ObjectNull, "未发现配置信息！");
                };

                return getFunc.WithCache(string.Concat(CacheKeys.Plugs_Config_ByKey, key), TimeSpan.FromHours(1));
            }
            catch (Exception ex)
            {
                if (!isFromLogModule)
                {
                     LogHelper.Error($"发送信息出错,错误信息：{ex.Message}", this.GetType().Name);
                }
            }
            return Task.FromResult(new Resp<TConfig>().WithResp(RespTypes.ObjectNull, "未发现配置信息！")); ;
        }

        
    }
}
