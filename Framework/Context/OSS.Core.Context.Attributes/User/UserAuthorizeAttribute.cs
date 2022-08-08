using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes.Common;

namespace OSS.Core.Context.Attributes;

/// <summary>
///  服务接口用户校验
/// </summary>
public class UserAuthorizeAttribute : BaseOrderAuthorizeAttribute
{
    private readonly UserAuthOption _userOption;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userOption"></param>
    public UserAuthorizeAttribute(UserAuthOption userOption)
    {
        Order       = AttributeConst.Order_User_AuthAttributeOrder;
        _userOption = userOption;
    }

    /// <summary>
    ///  授权异步处理
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<IResp> Authorize(AuthorizationFilterContext context)
    {
        if (CoreContext.User.IsAuthenticated)
            return Resp.DefaultSuccess;

        if (context.ActionDescriptor.EndpointMetadata.Any(filter => filter is IAllowAnonymous))
            return Resp.DefaultSuccess;

        var appInfo = CoreContext.App.Identity;

        var res = await FormatUserIdentity(_userOption);
        if (!res.IsSuccess())
        {
            return res;
        }

        return await CheckFunc(appInfo, _userOption);
    }

  


    private static async Task<IResp> FormatUserIdentity(UserAuthOption opt)
    {
        var identityRes = await opt.UserProvider.GetIdentity();
        if (!identityRes.IsSuccess())
            return identityRes;

        CoreContext.User.Identity = identityRes.data;
        return identityRes;
    }


    private static readonly Task<IResp> _successTaskResp = Task.FromResult((IResp) new Resp());

    private static Task<IResp> CheckFunc(AppIdentity appInfo, UserAuthOption opt)
    {
        if (opt.FuncProvider == null)
            return _successTaskResp;

        var askFunc = appInfo.ask_func;

        return opt.FuncProvider.Authorize(askFunc);
    }
}

/// <summary>
///  用户授权参数
/// </summary>
public class UserAuthOption
{
    public UserAuthOption(IUserAuthProvider userProvider)
    {
        UserProvider = userProvider ?? throw new Exception("UserAuthOption 中 UserProvider 接口对象必须提供！");
    }

    /// <summary>
    ///  功能方法权限判断接口
    /// </summary>
    public IFuncAuthProvider? FuncProvider { get; set; }

    /// <summary>
    ///  用户授权登录判断接口
    /// </summary>
    public IUserAuthProvider UserProvider { get; set; }
}