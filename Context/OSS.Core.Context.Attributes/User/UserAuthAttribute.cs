using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.BasicMos.Resp;

namespace OSS.Core.Context.Attributes
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

            var appInfo = context.HttpContext.InitialContextAppIdentity();
            var res     = await FormatUserIdentity(context, appInfo, _userOption);
            if (!res.IsSuccess())
            {
                ResponseExceptionEnd(context, appInfo, res);
                return;
            }

            res = await CheckFunc(context.HttpContext, appInfo, _userOption);
            if (!res.IsSuccess())
                ResponseExceptionEnd(context, appInfo, res);
        }


        private static async Task<Resp> FormatUserIdentity(AuthorizationFilterContext context, AppIdentity appInfo, UserAuthOption opt)
        {
            var identityRes = await opt.UserProvider.GetIdentity(context.HttpContext, appInfo);
            if (!identityRes.IsSuccess())
                return identityRes;

            CoreUserContext.SetIdentity(identityRes.data);
            return identityRes;
        }

        private static async Task<Resp> CheckFunc(HttpContext context, AppIdentity appInfo, UserAuthOption opt)
        {
            var userInfo = CoreUserContext.Identity;
            if (userInfo == null // 非需授权认证请求
                || opt.FuncProvider == null 
                || appInfo.ask_func == null
                || userInfo.auth_type == PortalAuthorizeType.SuperAdmin)
                return new Resp<FuncDataLevel>(FuncDataLevel.All);
            
            var askFunc = appInfo.ask_func;
            if (userInfo.auth_type > askFunc.auth_type)
                return new Resp(RespTypes.UserNoPermission, "当前用户权限不足");

            var checkRes = await opt.FuncProvider.FuncAuthorize(context, userInfo, askFunc);
            if (!checkRes.IsSuccess())
                return checkRes;
            
            return checkRes;
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
