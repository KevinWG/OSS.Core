using Newtonsoft.Json;
using OSS.Common.Resp;
using OSS.Core.Domain;
using OSS.Tools.Cache;
using OSS.Tools.DirConfig;
using System.Data;

namespace OSS.Core.Comp.DirConfig.Mysql
{
    internal class ListConfigMysqlTool : IToolListConfig
    {
        private readonly ConfigRep _configRep;

        public ListConfigMysqlTool(ConnectionOption opt)
        {
            _configRep = new ConfigRep(opt);
        }

        private const string System_ConfigList_ByKey = "System_Config_{0}";
        public async Task<bool> SetItem<TConfig>(string listKey, string itemKey, TConfig itemValue, string sourceName)
        {
            var listCacheKey = string.Format(System_ConfigList_ByKey, listKey);
            var res          = await AddOrUpdateItem(listKey, itemKey, itemValue);
            if (res)
            {
                await CacheHelper.RemoveAsync(listCacheKey,sourceName);
            }

            return true;
        }


        private async Task<bool> AddOrUpdateItem<TConfig>(string listKey, string itemKey, TConfig itemValue)
        {
            var confJson  = JsonConvert.SerializeObject(itemValue);
            var updateRes = await _configRep.UpdateVal(listKey, itemKey, confJson);
            if (updateRes.IsSuccess())
                return true;

            var getRes = await _configRep.GetByKey(listKey, itemKey);
            if (!getRes.IsRespCode(RespCodes.OperateObjectNull))
                return false;

            var mo = new ConfigMo
            {
                list_key = listKey,
                item_key = itemKey,
                item_val = confJson
            };

            mo.FormatBaseByContext();
            await _configRep.Add(mo);
            return true;
        }

        public async Task<List<ItemConfig<TConfig>>> GetList<TConfig>(string listKey, string sourceName)
        {
            var listCacheKey = string.Format(System_ConfigList_ByKey, listKey);
            var listRes =await _configRep.GetListByKey(listKey);

            var configList = listRes.Select(item => new ItemConfig<TConfig>()
                {key = item.item_key, value = JsonConvert.DeserializeObject<TConfig>(item.item_val)}).ToList();

            await CacheHelper.SetAbsoluteAsync(listCacheKey, configList, TimeSpan.FromHours(2), sourceName);
            return configList;
        }

        public Task<int> GetCount(string listKey, string sourceName)
        {
            return _configRep.GetCount(listKey);
        }

        public async Task<ItemConfig<TConfig>?> GetItem<TConfig>(string listKey, string itemKey, string sourceName)
        {
            var itemRes =await _configRep.GetByKey(listKey, itemKey);
            return itemRes.IsSuccess() ? new ItemConfig<TConfig>(){key = itemKey,value = JsonConvert.DeserializeObject<TConfig>(itemRes.data.item_val) } : null;
        }

        public Task RemoveItem(string listKey, string itemKey, string sourceName)
        {
            throw new RespException(RespCodes.OperateFailed, "系统配置不得移除！");
        }
    }
}