#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore FrontWeb —— 接口请求类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-17
*       
*****************************************************************************/

#endregion

using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OSS.Common.ComModels;
using OSS.Core.Infrastructure.Utils;
using OSS.Http.Mos;

namespace OSS.Core.WebSite.App_Start
{
    public static class ApiManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TReq"></typeparam>
        /// <typeparam name="TRep"></typeparam>
        /// <param name="req"></param>
        /// <returns></returns>
        public static async Task<TRep> PostApi<TReq, TRep>(TReq req)
            where TRep : ResultMo, new()
        {
            var osReq = new OsHttpRequest();

            osReq.HttpMothed = HttpMothed.POST;
            osReq.RequestSet = r => r.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            osReq.CustomBody = JsonConvert.SerializeObject(req);

            return await osReq.RestCommon<TRep>();
        }
    }
}
