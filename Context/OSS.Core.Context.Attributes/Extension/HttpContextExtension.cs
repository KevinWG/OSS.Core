using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos;
using OSS.Common.Extension;
using OSS.Core.Context.Attributes.Helper;

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
            var headerName = InterReqHelper.Option?.JSRequestHeaderName;
            if (!string.IsNullOrEmpty(headerName)&& req.Headers.ContainsKey(headerName))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///  将请求URL参数转化搜索请求对象
        /// </summary>
        /// <param name="reqQuery"></param>
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
                    else
                    {
                        searchReq.filter[para.Key] = para.Value.ToString();
                    }
                }
            }
            return searchReq;
        }
        
        ///// <summary>
        /////  Form表单请求转化搜索请求对象
        ///// </summary>
        ///// <param name="reqForm"></param>
        ///// <returns></returns>
        //public static SearchReq ToSearchReq(this IFormCollection reqForm)
        //{
        //    var searchReq = new SearchReq();
        //    foreach (var para in reqForm)
        //    {
        //        if (!string.IsNullOrEmpty(para.Value))
        //        {
        //            if (para.Key == "size")
        //            {
        //                searchReq.size = para.Value.ToString().ToInt32();
        //            }
        //            else if (para.Key == "page")
        //            {
        //                searchReq.page = para.Value.ToString().ToInt32();
        //            }
        //            else
        //            {
        //                searchReq.filter[para.Key] = para.Value.ToString();
        //            }
        //        }
        //    }
        //    return searchReq;
        //}
    }
}
