#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 对OSS.Core.WebApi 接口请求帮助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OSS.Common.ComModels;
using OSS.Core.Infrastructure.Utils;
using OSS.Http.Mos;

namespace OSS.Core.WebSite.AppCodes
{
    public static class ApiUtil
    {

        private static readonly string apiUrlPre = ConfigUtil.GetSection("ApiConfig:BaseUrl").Value;

        /// <summary>
        ///   post一个Api请求
        /// </summary>
        /// <typeparam name="TReq"></typeparam>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="apiPath"></param>
        /// <param name="req"></param>
        /// <param name="funcFormat"></param>
        /// <returns></returns>
        public static async Task<TRes> PostApi<TReq, TRes>(string apiPath, TReq req = null, Func<HttpResponseMessage, Task<TRes>> funcFormat = null)
            where TReq : class
            where TRes : ResultMo, new()
        {

            var httpReq = new OsHttpRequest
            {
                HttpMothed = HttpMothed.POST,
                AddressUrl = string.Concat(apiUrlPre, apiPath),
                CustomBody = JsonConvert.SerializeObject(req)
            };

            return await httpReq.RestApiCommon<TRes>();
        }
    }
}
