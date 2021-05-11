#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

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
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Helpers;
using OSS.Core.Infrastructure.Web.Helpers;
using OSS.Tools.Config;
using OSS.Tools.Http.Extention;
using OSS.Tools.Http.Mos;
using OSS.Tools.Log;

namespace OSS.CorePro.AdminSite.AppCodes
{
    /// <summary>
    ///     请求api辅助类
    /// </summary>
    public static class RestApiHelper
    {
        private static readonly string coreApiUrlPre = ConfigHelper.GetSection("Service:CoreApi").Value;

        #region 接口请求返回 Resp继承类 通用请求方法

        /// <summary>
        ///     post一个Api请求
        /// </summary>
        /// <typeparam name="TRes"></typeparam>
        /// <param name="apiRoute"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public static async Task<TRes> PostApi<TRes>(string apiRoute, object req = null)
            where TRes : Resp, new()
        {
            var apiUrl = string.Concat(coreApiUrlPre, apiRoute);
            return await Rest<TRes>(apiUrl, req, HttpMethod.Post);
        }

        public static async Task<TRes> GetApi<TRes>(string apiRoute)
            where TRes : Resp, new()
        {
            var apiUrl = string.Concat(coreApiUrlPre, apiRoute);
            return await Rest<TRes>(apiUrl, null, HttpMethod.Get);
        }

        public static async Task<Resp<TRes>> PostEntApi<TRes>(string apiRoute, object req = null)
        {
            var apiUrl = string.Concat(coreApiUrlPre, apiRoute);
            return await Rest<Resp<TRes>>(apiUrl, req, HttpMethod.Post);
        }
        public static async Task<Resp<TRes>> GetEntApi<TRes>(string apiRoute)
        {
            var apiUrl = string.Concat(coreApiUrlPre, apiRoute);
            return await Rest<Resp<TRes>>(apiUrl, null, HttpMethod.Get);
        }


        private static async Task<TRes> Rest<TRes>(string absoluateApiUrl, object reqContent, HttpMethod mothed)
            where TRes : Resp, new()
        {
            var httpReq = new OssHttpRequest
            {
                HttpMethod = mothed,
                AddressUrl = absoluateApiUrl,
                CustomBody = reqContent == null
                    ? null
                    : JsonConvert.SerializeObject(reqContent, Formatting.None, new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling    = NullValueHandling.Ignore
                    }),
                RequestSet = SetReqestFormat
            };

            return await Rest<TRes>(httpReq, JsonFormat<TRes>) ??
                new TRes() { ret = (int) RespTypes.InnerError, msg = "应用接口响应失败！" };
        }

        private static async Task<TRes> JsonFormat<TRes>(HttpResponseMessage resp)
            where TRes : Resp, new()
        {
            if (!resp.IsSuccessStatusCode)
                return new TRes()
                {
                    ret = -(int) resp.StatusCode,
                    msg = resp.ReasonPhrase
                };
            var contentStr = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TRes>(contentStr);
        }

        #endregion
        
        #region 接口请求返回 String 通用请求方法

        public static async Task<string> PostStrApi(string apiRoute, string req = null)
        {
            var apiUrl = string.Concat(coreApiUrlPre, apiRoute);
            return await RestStr(apiUrl, req, HttpMethod.Post);
        }
        public static async Task<string> GetStrApi(string apiRoute)
        {
            var apiUrl = string.Concat(coreApiUrlPre, apiRoute);
            return await RestStr(apiUrl, null, HttpMethod.Get);
        }

        private static async Task<string> StrFormat(HttpResponseMessage resp)
        {
            if (!resp.IsSuccessStatusCode)
                return $"{{\"ret\":{-(int) resp.StatusCode},\"msg\":\"{resp.ReasonPhrase}\"}}";

            var resStr = await resp.Content.ReadAsStringAsync();
            return resStr;
        }
        private static async Task<string> RestStr(string absoluateApiUrl, string reqContent, HttpMethod mothed)
        {
            var httpReq = new OssHttpRequest
            {
                HttpMethod = mothed,
                AddressUrl = absoluateApiUrl,
                CustomBody = reqContent,
                RequestSet = SetReqestFormat
            };

            return await Rest(httpReq, StrFormat) ?? $"{{\"ret\":{(int) RespTypes.InnerError},\"msg\":\"应用接口响应失败！\"}}";
        }

        #endregion
        
        private static void SetReqestFormat(HttpRequestMessage r)
        {
            var ticket = AppReqContext.Identity.ToTicket(AppInfoHelper.AppId,AppInfoHelper.AppVersion, AppInfoHelper.AppSecret);

            r.Headers.Add(AppWebInfoHelper.ServerSignModeHeaderName, ticket);
            r.Headers.Add("Accept", "application/json");

            if (r.Content!=null)
                r.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/json") { CharSet = "UTF-8" };
        }


        private static async Task<TRes> Rest<TRes>(OssHttpRequest req, Func<HttpResponseMessage, Task<TRes>> format)
        {
            try
            {
                using (var resp = await req.RestSend())
                {
                    if (!resp.IsSuccessStatusCode)
                    {
                        LogHelper.Error($"错误信息：接口请求响应状态异常(HttpStatusCode:{resp.StatusCode},地址：{req.AddressUrl})！", "RestApi");
                    }
                    return await format(resp);
                }
            }
            catch (Exception error)
            {
                LogHelper.Error(string.Concat("错误信息：", error.Message, "详细信息：", error.StackTrace), nameof(RestApiHelper));
            }
            return default;
        }

    }
}