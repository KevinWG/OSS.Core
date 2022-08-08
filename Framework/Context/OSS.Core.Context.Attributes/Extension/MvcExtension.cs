using Microsoft.AspNetCore.Mvc;

namespace OSS.Core.Context.Attributes
{
    public static class MvcExtension
    {
        /// <summary>
        ///  添加应用层级授权验证过滤器
        /// </summary>
        public static void AddCoreAppAuthorization(this MvcOptions opt, IAppAuthProvider? appAuthProvider =null, ITenantAuthProvider? tenantAuthProvider =null)
        {
            opt.Filters.Add(new AppAuthorizeAttribute(new AppAuthOption()
            {
                AppAuthProvider    = appAuthProvider,
                TenantAuthProvider = tenantAuthProvider
            }));
        }


        /// <summary>
        ///  添加用户层级授权验证过滤器
        /// </summary>
        public static void AddCoreUserAuthorization(this MvcOptions opt, IUserAuthProvider userAuthProvider, IFuncAuthProvider? funcAuthProvider = null)
        {
            opt.Filters.Add(new UserAuthorizeAttribute(new UserAuthOption(userAuthProvider)
            {
                FuncProvider = funcAuthProvider
            }));
        }

        /// <summary>
        ///  添加参数模型约束验证过滤器
        /// </summary>
        public static void AddCoreModelValidation(this MvcOptions opt)
        {
            opt.Filters.Add(new ModelValidationAttribute());
        }
    }
}
