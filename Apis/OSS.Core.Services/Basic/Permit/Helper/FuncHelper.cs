using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;
using OSS.Common.BasicMos.Resp;
using OSS.Tools.DirConfig;
using OSS.Core.AdminSite.Apis.Permit.Reqs;

namespace OSS.Core.AdminSite.Apis.Permit.Helpers
{
    public class FuncHelper
    {
        private static readonly DefaultToolDirConfig _xmlConfig = new DefaultToolDirConfig();

        #region 权限配置信息

        private static List<FuncBigItem> _configFuncItems;

        /// <summary>
        ///  获取系统菜单权限信息
        ///  todo  需结合租户模块获取租户下的真实权限列表（同时修改到缓存-区分租户）
        /// </summary>
        /// <returns></returns>
        public static Task<ListResp<FuncBigItem>> GetAllFuncItems()
        {
            if (_configFuncItems != null)
                return Task.FromResult(new ListResp<FuncBigItem>(_configFuncItems));

            var siteConfig = _xmlConfig.GetDirConfig<SiteFuncConfig>("sys_func_list").Result;
            if (siteConfig == null)
                throw new NullReferenceException("未发现初始权限配置信息(sys_func_list.config)");

            _configFuncItems = new List<FuncBigItem>();

            RecurInitialFunBigItems(_configFuncItems, siteConfig.items, null);
            return Task.FromResult(new ListResp<FuncBigItem>(_configFuncItems));
        }

        private static void RecurInitialFunBigItems(List<FuncBigItem> rList, List<FuncConfigItem> subItems,
            string parentCode)
        {
            if (subItems == null || subItems.Count <= 0)
                return;

            foreach (var sItem in subItems)
            {
                RecurInitialFunBigItems(rList, sItem.subitems, sItem.code);

                var item = sItem.ConvertToBigItem();
                item.parent_code = parentCode;

                rList.Add(item);
            }
        }
        #endregion
    }

    public class SiteFuncConfig
    {
        [XmlArrayItem("item")]
        public List<FuncConfigItem> items { get; set; }
    }

    public class FuncConfigItem : FuncItem
    {
        [XmlArray("subitems")]
        [XmlArrayItem("item")]
        public List<FuncConfigItem> subitems { get; set; }
    }

    public static class FuncConfigItemMaps
    {
        public static FuncBigItem ConvertToBigItem(this FuncConfigItem configItem)
        {
            var item = new FuncBigItem
            {
                code = configItem.code,
                title = configItem.title
            };
            return item;
        }
    }
}
