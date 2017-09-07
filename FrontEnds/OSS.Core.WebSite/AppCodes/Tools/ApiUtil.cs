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
using OSS.Core.Infrastructure.Utils;
using OSS.Http.Extention;
using OSS.Http.Mos;

namespace OSS.Core.WebSite.AppCodes.Tools
{

    /// <summary>
    ///  请求api辅助类
    /// </summary>
    public static class ApiUtil
    {

        private static readonly string coreApiUrlPre = ConfigUtil.GetSection("CoreApiConfig:BaseUrl").Value;

        /// <summary>
        ///   post一个Api请求
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <typeparam name="TReq"></typeparam>
        /// <param name="apiRoute"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public static async Task<TRes> PostCoreApi<TReq,TRes>(string apiRoute, TReq req = null)
            where TReq : class 
            where TRes : ResultMo, new()
        {
            var apiUrl = string.Concat(coreApiUrlPre, apiRoute);
            var reqContent = JsonConvert.SerializeObject(req, Formatting.None, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            
            return await PostApi<TRes>(apiUrl, reqContent);
        }

        public static async Task<TRes> PostApi<TRes>(string absoluateApiUrl, string reqContent)
            where TRes : ResultMo, new()
        {
            var sysInfo = MemberShiper.AppAuthorize;

            var secretKeyRes = ApiSourceKeyUtil.GetAppSecretKey(sysInfo.AppSource);
            if (!secretKeyRes.IsSuccess())
                return secretKeyRes.ConvertToResult<TRes>();

            var httpReq = new OsHttpRequest
            {
                HttpMothed = HttpMothed.POST,
                AddressUrl = absoluateApiUrl,
                CustomBody = reqContent,

                RequestSet = r =>
                {
                    r.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json") { CharSet = "UTF-8" };
                    var ticket = MemberShiper.AppAuthorize.ToSignData(secretKeyRes.data);
                    r.Content.Headers.Add(GlobalKeysUtil.AuthorizeTicketName, ticket);
                }
            };

            return await httpReq.RestCommonJson<TRes>();
        }
    }
}