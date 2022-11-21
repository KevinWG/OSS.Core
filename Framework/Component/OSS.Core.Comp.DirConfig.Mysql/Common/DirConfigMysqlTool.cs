using OSS.Tools.DirConfig;

namespace OSS.Core.Comp.DirConfig.Mysql
{
    internal class DirConfigMysqlTool:IToolDirConfig
    {
        private readonly ListConfigMysqlTool _listTool;

        public DirConfigMysqlTool(ConnectionOption opt)
        {
            _listTool = new ListConfigMysqlTool(opt);
        }

        public Task<bool> SetDirConfig<TConfig>(string key, TConfig dirConfig, string sourceName)
        {
            return _listTool.SetItem(key, string.Empty, dirConfig, sourceName);
        }

        public Task RemoveDirConfig(string key, string sourceName)
        {
            return _listTool.RemoveItem(key, string.Empty, sourceName);
        }

        public  async Task<TConfig?> GetDirConfig<TConfig>(string key, string sourceName)
        {
            var listItem = await _listTool.GetItem<TConfig>(key, string.Empty, sourceName);
            return listItem==null ? default : listItem.value;
        }
    }
}