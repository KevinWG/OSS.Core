//using Microsoft.AspNetCore.Mvc.Filters;
//using OSS.Common.Resp;
//using OSS.Core.Context;
//using OSS.Core.Context.Attributes;
//using System.Threading.Tasks;
//using OSS.Core.Common;

//namespace OSS.Core.WebApis.App_Codes.Filter
//{
//    /// <summary>
//    ///  通行验证过滤器
//    /// </summary>
//    public class PassTokenValidatorAttribute : BaseAuthAttribute
//    {
//        private string _queryKeyName;
//        private string _queryTokenName;

//        /// <summary>
//        /// 通行验证过滤器
//        /// </summary>
//        /// <param name="queryKeyName"></param>
//        /// <param name="queryTokenName"></param>
//        public PassTokenValidatorAttribute(string queryKeyName = "id", string queryTokenName = "p_t")
//        {
//            _queryKeyName = queryKeyName;
//            _queryTokenName = queryTokenName;
//        }


//        /// <inheritdoc />
//        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
//        {
//            if (!CoreUserContext.IsAuthenticated)
//            {
//                ResponseExceptionEnd(context, CoreContext.App.Identity, new Resp(RespTypes.UserNoPermission, "没有当前地址访问权限!"));
//                return Task.CompletedTask;
//            }

//            var keyValue = GetFromQuery(context, _queryKeyName);
//            var passToken = GetFromQuery(context, _queryTokenName);

//            if (string.IsNullOrEmpty(keyValue) || string.IsNullOrEmpty(passToken) || !PassTokenHelper.CheckToken(keyValue, passToken))
//            {
//                ResponseExceptionEnd(context, CoreContext.App.Identity, new Resp(RespTypes.UserNoPermission, "没有当前地址访问权限!"));
//            }
//            return Task.CompletedTask;
//        }

//        private static string GetFromQuery(AuthorizationFilterContext context, string name)
//        {
//            if (context.RouteData.Values.TryGetValue(name, out var val))
//            {
//                return val?.ToString();
//            }
//            return context.HttpContext.Request.Query.TryGetValue(name, out var rVal) ? rVal.ToString() : string.Empty;
//        }
//    }
//}
