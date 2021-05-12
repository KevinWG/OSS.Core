#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 应用交互Key辅助类
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
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;
using OSS.Tools.Config;

namespace OSS.Core.CoreApi.App_Codes.AuthProviders
{
    public class AppAuthProvider : IAppAuthProvider
    {
        public Task<Resp> AppAuthCheck(HttpContext context, AppIdentity appinfo)
        {
            if (appinfo.SourceMode == AppSourceMode.ServerSign)
            {
                var key = ConfigHelper.GetSection("KnockAppSecrets:" + appinfo.app_id)?.Value;

                const int expireSecs = 60 * 60 * 2;
                if (!appinfo.CheckSign(key, expireSecs).IsSuccess())
                    return Task.FromResult(new Resp(RespTypes.SignError, "签名错误！"));
            }

            return Task.FromResult(new Resp());
        }

    }
}