using Microsoft.AspNetCore.Mvc;

namespace OSS.Core.Context.Attributes
{
    public static class MvcExtension
    {      /// <summary>
        ///  添加应用层级授权验证过滤器
        /// </summary>
        public static void AddCoreAppAuthorization(this MvcOptions opt, AppAuthOption appAuthOpt)
        {
            opt.Filters.Add(new AppAuthorizeAttribute(appAuthOpt));
        }

        /// <summary>
        ///  添加应用层级授权验证过滤器
        /// </summary>
        public static void AddCoreAppAuthorization(this MvcOptions opt, IAppAccessProvider? signAccessProvider =null, ITenantAuthProvider? tenantAuthProvider =null)
        {
            opt.Filters.Add(new AppAuthorizeAttribute(new AppAuthOption()
            {
                SignAccessProvider = signAccessProvider,
                TenantAuthProvider = tenantAuthProvider
            }));
        }


        /// <summary>
        ///  添加用户层级授权验证过滤器
        /// </summary>
        public static void AddCoreUserAuthorization(this MvcOptions opt, UserAuthOption userAuthOpt)
        {
            opt.Filters.Add(new UserAuthorizeAttribute(userAuthOpt));
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
