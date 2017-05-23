#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— http接口请求辅助方法
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
using OSS.Common.ComModels.Enums;
using OSS.Common.Modules;
using OSS.Common.Plugs.LogPlug;
using OSS.Http;
using OSS.Http.Mos;

namespace OSS.Core.Infrastructure.Utils
{
    public static class RestUtil
    {
        /// <summary>
        /// 处理远程请求方法，并返回需要的实体
        /// </summary>
        /// <typeparam name="T">需要返回的实体类型</typeparam>
        /// <param name="request">远程请求组件的request基本信息</param>
        /// <param name="funcFormat">获取实体格式化方法</param>
        /// <returns>实体类型</returns>
        public static async Task<T> RestCommon<T>(this OsHttpRequest request,
            Func<HttpResponseMessage, Task<T>> funcFormat = null)
            where T : ResultMo, new()
        {
            T t = default(T);
            try
            {
                var resp = await request.RestSend();
                if (resp.IsSuccessStatusCode)
                {
                    if (funcFormat != null)
                        t = await funcFormat(resp);
                    else
                    {
                        var contentStr = await resp.Content.ReadAsStringAsync();
                        t = JsonConvert.DeserializeObject<T>(contentStr);
                    }
                }
            }
            catch (Exception ex)
            {
                t = new T() { Ret = (int)ResultTypes.InnerError, Message = ex.Message };
                LogUtil.Error(string.Concat("基类请求出错，错误信息：", ex.Message), "RestCommon", ModuleNames.SocialCenter);
            }
            return t ?? new T() { Ret = 0 };
        }

    }
}
