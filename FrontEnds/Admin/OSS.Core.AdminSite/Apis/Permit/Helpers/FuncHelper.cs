using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using OSS.Common.BasicMos.Resp;   
using OSS.Core.Context;
using OSS.Core.Context.Mos;

using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Extensions;
using OSS.CorePro.AdminSite.AppCodes;
using OSS.CorePro.TAdminSite.Apis.Permit.Reqs;
using OSS.Tools.Cache;
using OSS.Tools.DirConfig;

namespace OSS.CorePro.TAdminSite.Apis.Permit.Helpers
{
    public class FuncHelper
    {
        #region 权限配置信息

        private static List<FuncBigItem> _configFuncItems;

        /// <summary>
        ///  获取系统菜单权限信息
        ///  todo  需结合租户模块获取租户下的真实权限列表（同时修改到缓存-区分租户）
        /// </summary>
        /// <returns></returns>
        public static  Task<ListResp<FuncBigItem>> GetAllFuncItems()
        {
            if (_configFuncItems != null)
                return Task.FromResult(new ListResp<FuncBigItem>(_configFuncItems));

            var siteConfig = DirConfigHelper.GetDirConfig<SiteFuncConfig>("sys_func_list").Result;
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
        
        #region 登录用户权限处理

        /// <summary>
        ///  判断登录用户是否具有某权限
        /// </summary>
        /// <param name="funcCode"></param>
        /// <returns></returns>
        public static async Task<Resp> CheckAuthUserIfHaveFunc(string funcCode)
        {
            var memIdentity = UserContext.Identity;
            if (memIdentity.auth_type == PortalAuthorizeType.SuperAdmin)
                return new Resp();

            var userFunc = await GetAuthUserRoleFuncList();
            if (!userFunc.IsSuccess() || userFunc.data.All(f => f.func_code != funcCode))
                return new Resp().WithResp(RespTypes.NoPermission, "无此权限！");

            return new Resp();
        }

        /// <summary>
        ///  获取登录用户的所有权限列表
        ///     这里有 30分钟的缓存误差（退出登录时会清理），基础接口层有10分钟的缓存误差
        /// </summary>
        /// <returns></returns>
        public static Task<ListResp<GetRoleItemResp>> GetAuthUserRoleFuncList()
        {
            var memIdentity = UserContext.Identity;

            var key = string.Concat(CoreCacheKeys.Perm_UserFuncs_ByUId, memIdentity.id);

            Func<Task<ListResp<GetRoleItemResp>>> getFunc = GetAuthUserFuncListFromApi;
            return getFunc.WithAbsoluteCache(key, TimeSpan.FromMinutes(30));
        }
        /// <summary>
        /// 清理登录用户的权限缓存
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Task<bool> ClearAuthUserFuncListCache(UserIdentity user)
        {
            var key = string.Concat(CoreCacheKeys.Perm_UserFuncs_ByUId, user.id);
            return CacheHelper.RemoveAsync(key);
        }

        // 接口层有十分钟左右的缓存误差
        private static async Task<ListResp<GetRoleItemResp>> GetAuthUserFuncListFromApi()
        {
            var memIdentity = UserContext.Identity;
            if (memIdentity.auth_type != PortalAuthorizeType.SuperAdmin)
                return await RestApiHelper.GetApi<ListResp<GetRoleItemResp>>("/b/permit/GetAuthUserFuncList");

            // 如果是超级管理员直接返回所有
            var sysFunItemsRes = await GetAllFuncItems();
            if (!sysFunItemsRes.IsSuccess())
                return new ListResp<GetRoleItemResp>().WithResp(sysFunItemsRes);

            var roleItems = sysFunItemsRes.data.Select(item => item.ToRoleItemResp()).ToList();
            return new ListResp<GetRoleItemResp>(roleItems);
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
