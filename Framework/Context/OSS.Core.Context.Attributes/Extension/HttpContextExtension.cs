﻿using Microsoft.AspNetCore.Http;
using OSS.Common;
using OSS.Common.Extension;


namespace OSS.Core.Context.Attributes
{
    /// <summary>
    ///  属性扩展
    /// </summary>
    public static class HttpContextExtension
    {
        /// <summary>
        ///  当站点使用服务端页面渲染，如果配置了   异常时会跳转
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static bool IsFetchApi(this HttpRequest req)
        {
            var headerName = InterReqHelper.Option.JSRequestHeaderName;
            return !string.IsNullOrEmpty(headerName) && req.Headers.ContainsKey(headerName);
        }

        /// <summary>
        ///  将请求URL参数转化搜索请求对象
        /// </summary>
        /// <param name="reqQuery">排除"sort"字段，或者 '_' 开头</param>
        /// <returns></returns>
        public static SearchReq ToSearchReq(this IQueryCollection reqQuery)
        {
            var searchReq = new SearchReq();
            foreach (var para in reqQuery)
            {
                if (!string.IsNullOrEmpty(para.Value))
                {
                    if (para.Key == "size")
                    {
                        searchReq.size = para.Value.ToString().ToInt32();
                    }
                    else if (para.Key == "page")
                    {
                        searchReq.page = para.Value.ToString().ToInt32();
                    }
                    else if (para.Key != "sort" || !para.Key.StartsWith("_"))
                    {
                        searchReq.filter[para.Key] = para.Value.ToString();
                    }
                }
            }
            return searchReq;
        }

        /// <summary>
        ///  初始化当前请求上下文的应用全局信息
        /// </summary>
        /// <returns></returns>
        internal static AppIdentity GetOrInitialCoreAppIdentity(this HttpContext context)
        {
            if (CoreContext.App.IsInitialized)
                return CoreContext.App.Identity;

            var identity = new AppIdentity();
            CoreContext.App.Identity = identity;

            return identity;
        }

    }
}