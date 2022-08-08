using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using OSS.Common.Resp;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    /// 授权基类
    /// </summary>
    public abstract class BaseOrderAuthorizeAttribute : BaseAuthorizeAttribute, IOrderedFilter
    {
        /// <summary>
        ///  执行顺序
        /// </summary>
        public int Order { get; set; }
    }

    /// <summary>
    ///  基础验证属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class BaseAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        /// <summary>
        /// 授权方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var appIdentity = CoreContext.App.Identity;

            var authRes     = await Authorize(context);

            if (!authRes.IsSuccess())
            {
                ResponseExceptionEnd(context, appIdentity, authRes);
            }
        }


        public abstract Task<IResp> Authorize(AuthorizationFilterContext context);

        /// <summary>
        ///   异常结束响应
        /// </summary>
        /// <param name="context"></param>
        /// <param name="appInfo"></param>
        /// <param name="res"></param>
        private static void ResponseExceptionEnd(AuthorizationFilterContext context, AppIdentity appInfo, IResp res)
        {
            var rUrl = res.IsRespCode(RespCodes.UserUnLogin) 
                ? InterReqHelper.GetUnloginPage(context.HttpContext, appInfo) 
                : InterReqHelper.GetErrorPage(context.HttpContext, appInfo, res);
            
            if (string.IsNullOrEmpty(rUrl))
            {
                context.Result = res.ToJsonResult();
                return;
            }

            context.Result = new RedirectResult(rUrl);
            return;
        }
        
    }



}
