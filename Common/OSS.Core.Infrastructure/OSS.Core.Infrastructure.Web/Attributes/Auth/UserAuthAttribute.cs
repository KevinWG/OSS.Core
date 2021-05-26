using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Extention;
using OSS.Core.Context;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.Web.Attributes.Auth.Interface;
using OSS.Core.Infrastructure.Web.Helpers;

namespace OSS.Core.Infrastructure.Web.Attributes.Auth
{
    /// <summary>
    ///  服务接口用户校验
    /// </summary>
    public class UserAuthAttribute : BaseOrderAuthAttribute
    {
        private readonly UserAuthOption _userOption;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userOption"></param>
        public UserAuthAttribute(UserAuthOption userOption)
        {
            if (userOption?.UserProvider == null)
                throw new Exception("UserAuthOption 中 UserProvider 接口对象必须提供！");

            p_Order     = -10;
            _userOption = userOption;
            //p_IsWebSite = userOption.IsWebSite;
        }

        /// <summary>
        ///  授权异步处理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (CoreUserContext.IsAuthenticated)
                return;

            if (context.ActionDescriptor.EndpointMetadata.Any(filter => filter is IAllowAnonymous))
                return;

            var appInfo = CoreAppContext.Identity;
            //_userOption.UserProvider.FormatUserToken(context.HttpContext, appInfo);

            var res = await FormatUserIdentity(context, appInfo, _userOption);
            if (!res.IsSuccess())
            {
                UserAuthErrorReponse(context, appInfo, res);
                return;
            }

            res = await CheckFunc(context.HttpContext, appInfo, _userOption);
            if (!res.IsSuccess())
            {
                ResponseExceptionEnd(context, res);
            }
        }

        private void UserAuthErrorReponse(AuthorizationFilterContext context, AppIdentity appInfo, Resp res)
        {
            if (!res.IsRespType(RespTypes.UnLogin))
            {
                ResponseExceptionEnd(context, res);
                return;
            }

            // 重定向用户登录页
            if (!string.IsNullOrEmpty(AppWebInfoHelper.LoginUrl)
                   && appInfo.SourceMode == AppSourceMode.Browser)
            {
                var req = context.HttpContext.Request;
                var rUrl = string.Concat(req.Path, req.QueryString);

                var newUrl = string.Concat(AppWebInfoHelper.LoginUrl, "?rurl=" + rUrl.UrlEncode());

                context.Result = new RedirectResult(newUrl);
                return;
            }
            context.Result = new JsonResult(res);
            return;
        }

        private static async Task<Resp> FormatUserIdentity(AuthorizationFilterContext context,AppIdentity appInfo,UserAuthOption opt)
        {
            var identityRes = await opt.UserProvider.InitialIdentity(context.HttpContext, appInfo);
            if (!identityRes.IsSuccess())
                return identityRes;

            CoreUserContext.SetIdentity(identityRes.data);
            return identityRes;
        }

        private static Task<Resp> CheckFunc(HttpContext context, AppIdentity appInfo, UserAuthOption opt)
        {
            var userInfo = CoreUserContext.Identity;
            if (userInfo == null // 非需授权认证请求
                || opt.FuncProvider == null 
                || userInfo.auth_type == PortalAuthorizeType.SuperAdmin)
                return Task.FromResult(new Resp());

            return opt.FuncProvider.CheckFuncPermission(context, userInfo, appInfo.ask_func);
        }

    }

    /// <summary>
    ///  用户授权参数
    /// </summary>
    public class UserAuthOption 
    {
        /// <summary>
        ///  功能方法权限判断接口
        /// </summary>
        public IFuncAuthProvider FuncProvider { get; set; }

        /// <summary>
        ///  用户授权登录判断接口
        /// </summary>
        public IUserAuthProvider UserProvider { get; set; }

    }
}
