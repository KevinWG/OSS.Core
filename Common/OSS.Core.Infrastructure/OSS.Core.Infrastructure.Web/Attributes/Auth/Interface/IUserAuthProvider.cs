#region Copyright (C) 2018 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 成员信息公用类 （前后台用户信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2018-11-19
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Context.Mos;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth.Interface
{
    public interface IUserAuthProvider
    {
        /// <summary>
        ///  中间件初始化用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="appinfo"></param>
        /// <returns></returns>
        Task<Resp<UserIdentity>> InitialAuthUserIdentity(HttpContext context, AppIdentity appinfo);

        /// <summary>
        ///  处理用token
        /// </summary>
        /// <param name="context"></param>
        /// <param name="appinfo"></param>
        /// <returns></returns>
        void FormatUserToken(HttpContext context, AppIdentity appinfo);
    }
}
