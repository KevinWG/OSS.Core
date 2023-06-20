using Microsoft.AspNetCore.Mvc;

namespace OSS.Core.Context.Attributes;

public static class MvcExtension
{
    #region 应用授权
    
    /// <summary>
    ///  添加应用层级授权验证过滤器
    /// </summary>
    public static void AddCoreAppAuthorization(this MvcOptions opt, IAppAccessProvider? signAccessProvider = null)
    {
        opt.Filters.Add(new AppAuthorizeAttribute(new AppAuthOption()
        {
            SignAccessProvider = signAccessProvider
        }));
    }

    /// <summary>
    ///  添加应用层级授权验证过滤器
    /// </summary>
    public static void AddCoreAppAuthorization(this MvcOptions opt, AppAuthOption appAuthOpt)
    {
        opt.Filters.Add(new AppAuthorizeAttribute(appAuthOpt));
    }

    #endregion


    #region 租户授权扩展

    /// <summary>
    ///  添加租户层级授权验证过滤器
    /// </summary>
    public static void AddCoreTenantAuthorization(this MvcOptions opt,ITenantAuthProvider authProvider)
    {
        opt.Filters.Add(new TenantAuthorizeAttribute(authProvider));
    }

    #endregion


    #region 用户授权扩展

    /// <summary>
    ///  添加用户层级授权验证过滤器
    /// </summary>
    public static void AddCoreUserAuthorization(this MvcOptions opt, IUserAuthProvider userAuthProvider, UserAuthOption authOpt)
    {
        opt.Filters.Add(new UserAuthorizeAttribute(userAuthProvider, authOpt));
    }

    /// <summary>
    ///  添加用户层级授权验证过滤器
    /// </summary>
    public static void AddCoreUserAuthorization(this MvcOptions opt, IUserAuthProvider userAuthProvider, IFuncAuthProvider? funcAuthProvider = null)
    {
        opt.Filters.Add(new UserAuthorizeAttribute(userAuthProvider,new UserAuthOption()
        {
            FuncProvider = funcAuthProvider
        }));
    }
    
    #endregion


    /// <summary>
    ///  添加参数模型约束验证过滤器
    /// </summary>
    public static void AddCoreModelValidation(this MvcOptions opt)
    {
        opt.Filters.Add(new ModelValidationAttribute());
    }
}