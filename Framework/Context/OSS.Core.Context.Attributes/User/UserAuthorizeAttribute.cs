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

        var appInfo     = CoreContext.App.Identity;
        var identityRes = await _userOption.UserProvider.GetIdentity();
        if (!identityRes.IsSuccess())
        {
            return identityRes;
        }

        var userIdentity = identityRes.data;
        CoreContext.User.Identity = userIdentity;

        var funcRes      =await CheckFunc(appInfo, userIdentity, _userOption);
        if (funcRes.IsSuccess())
        {
            userIdentity.data_level = FuncDataLevel.All;
        }

        return funcRes;
    }

    

    private static async Task<IResp<FuncDataLevel>> CheckFunc(AppIdentity appInfo,UserIdentity userIdentity, UserAuthOption opt)
    {
        if (opt.FuncProvider == null)
            return new Resp<FuncDataLevel>(FuncDataLevel.All);

        var askFunc = appInfo.ask_func;
        if (userIdentity.auth_type == PortalAuthorizeType.SuperAdmin)
        {
            return new Resp<FuncDataLevel>(FuncDataLevel.All);
        }

        if (userIdentity.auth_type > askFunc.auth_type)
        {
            switch (userIdentity.auth_type)
            {
                case PortalAuthorizeType.SocialAppUser:
                    return new Resp<FuncDataLevel>().WithResp(RespCodes.UserFromSocial, "需要绑定系统账号");
                case PortalAuthorizeType.UserWithEmpty:
                    return new Resp<FuncDataLevel>().WithResp(RespCodes.UserIncomplete, "需要绑定手机号!");
            }
            return new Resp<FuncDataLevel>().WithResp(RespCodes.UserNoPermission, "权限不足!");
        }

        return await opt.FuncProvider.Authorize(askFunc);
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