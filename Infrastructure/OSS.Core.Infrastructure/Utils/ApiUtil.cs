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

using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Http.Extention;
using OSS.Http.Mos;

namespace OSS.Core.Infrastructure.Utils
{

    /// <summary>
    ///  请求api辅助类
    /// </summary>
    public static class ApiUtil
    {

        private static readonly string secretKey = ConfigUtil.GetSection("AppConfig:AppSecret").Value;
        private static readonly string coreApiUrlPre = ConfigUtil.GetSection("ApiUrlConfig:CoreApi").Value;
        private static readonly string snsApiUrlPre = ConfigUtil.GetSection("ApiUrlConfig:SnsApi").Value;


        /// <summary>
        ///   post一个Api请求
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="apiRoute"></param>
        /// <param name="req"></param>
        /// <param name="mothed">请求方式</param>
        /// <returns></returns>
        public static async Task<TRes> RestCoreApi<TRes>(string apiRoute, object req = null,
            HttpMothed mothed = HttpMothed.POST)
            where TRes : ResultMo, new()
        {
            var apiUrl = string.Concat(coreApiUrlPre, apiRoute);
            return await RestApi<TRes>(apiUrl, req, mothed);
        }

        /// <summary>
        ///   post一个Api请求
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="apiRoute"></param>
        /// <param name="req"></param>
        /// <param name="mothed">请求方式</param>
        /// <returns></returns>
        public static async Task<TRes> RestSnsApi<TRes>(string apiRoute, object req = null,
            HttpMothed mothed = HttpMothed.POST)
            where TRes : ResultMo, new()
        {
            var apiUrl = string.Concat(snsApiUrlPre, apiRoute);
            return await RestApi<TRes>(apiUrl, req, mothed);
        }


        public static async Task<TRes> RestApi<TRes>(string absoluateApiUrl, object reqContent, HttpMothed mothed)
            where TRes : ResultMo, new()
        {

            var httpReq = new OsHttpRequest
            {
                HttpMothed = mothed,
                AddressUrl = absoluateApiUrl,
                CustomBody = reqContent == null
                    ? null
                    : JsonConvert.SerializeObject(reqContent, Formatting.None, new JsonSerializerSettings()
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    }),

                RequestSet = r =>
                {
                    var ticket = MemberShiper.AppAuthorize.ToSignData(secretKey);
                    r.Headers.Add(GlobalKeysUtil.AuthorizeTicketName, ticket);
                    r.Headers.Add("Accept", "application/json");

                    if (r.Content != null)
                    {
                        r.Content.Headers.ContentType =
                            new MediaTypeHeaderValue("application/json") { CharSet = "UTF-8" };
                    }
                }
            };

            return await httpReq.RestCommonJson<TRes>();
        }
    }
}