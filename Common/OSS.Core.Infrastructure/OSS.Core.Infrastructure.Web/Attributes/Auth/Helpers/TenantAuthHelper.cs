#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 签名验证中间件
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context;
using OSS.Core.Context.Mos;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth.Helpers
{
    /// <summary>
    /// 租户的验证中间件
    /// </summary>
    public static class TenantAuthHelper 
    {
        public static async Task<Resp> FormatAndCheck(HttpContext context, AppIdentity appInfo, AppAuthOption appOption)
        {
            if (appInfo.is_partner|| appOption.TenantProvider==null|| TenantContext.Identity != null)
                return new Resp();
            
            // 服务api请求时，必须有值
            if (!appOption.IsWebSite && string.IsNullOrEmpty(appInfo.tenant_id))
                return new Resp().WithResp(RespTypes.ObjectNull, "未发现租户信息！");
            
            var identityRes = await appOption.TenantProvider.InitialAuthTenantIdentity(context, appInfo);
            if (!identityRes.IsSuccess())
                return identityRes;

            TenantContext.SetIdentity(identityRes.data);
            return identityRes;
        }

   

    }



}
