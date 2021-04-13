#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 用户信息公用类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-23
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;
using OSS.CorePro.TAdminSite.Apis.Portal.Helpers;

namespace OSS.CorePro.TAdminSite.AppCodes.Initial
{
    public class AdminAuthProvider : IUserAuthProvider
    {
        public void FormatUserToken(HttpContext context, AppIdentity appinfo)
        {
            if (string.IsNullOrEmpty(appinfo.token))
            {
                appinfo.token = AdminHelper.GetCookie(context.Request);
            }
        }
        public Task<Resp<UserIdentity>> InitialAuthUserIdentity(HttpContext context, AppIdentity appinfo)
        {
            return AdminHelper.GetAuthAdmin();
        }

    }
}
