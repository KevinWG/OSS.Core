using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes.Common;

namespace OSS.Core.Context.Attributes
{
    /// <summary>
    /// 接口请求模式验证
    /// </summary>
    public class WebFetchApiAttribute : BaseOrderAuthorizeAttribute
    {

        public WebFetchApiAttribute()
        {
            Order = -99999;
            //p_IsWebSite = true;
        }
        
        private static readonly Task<IResp> taskDefusedRes =
            Task.FromResult((IResp)new Resp(SysRespCodes.NotAllowed, "当前请求被拒绝!"));
        /// <inheritdoc />
        public override Task<IResp> Authorize(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.IsFetchApi())
            {
               return taskDefusedRes;
            }
            return AttributeConst.TaskSuccessResp ;
        }
    }


}
