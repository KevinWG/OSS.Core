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

    /// <inheritdoc />
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

        var userRes = await UserAuthorize(appInfo);
        if (!userRes.IsSuccess())
            return userRes;

        var userIdentity = userRes.data;
        return await FuncAuthorize(appInfo, userIdentity, _userOption);
    }


    private async Task<IResp<UserIdentity>> UserAuthorize(AppIdentity appIdentity)
    {
        var identityRes = await _userOption.UserProvider.GetIdentity();
        if (!identityRes.IsSuccess())
            return identityRes;

        var userIdentity = identityRes.data;
        if (userIdentity.auth_type > appIdentity.ask_auth.portal_auth_type)
        {
            switch (userIdentity.auth_type)
            {
                case PortalAuthorizeType.SocialAppUser:
                    return new Resp<UserIdentity>().WithResp(RespCodes.UserFromSocial, "需要绑定系统账号");
                case PortalAuthorizeType.UserWithEmpty:
                    return new Resp<UserIdentity>().WithResp(RespCodes.UserIncomplete, "需要绑定手机号!");
            }

            return new Resp<UserIdentity>().WithResp(RespCodes.UserNoPermission, "权限不足!");
        }

        CoreContext.User.Identity = userIdentity;
        return new Resp<UserIdentity>(userIdentity);
    }


    private static async Task<IResp> FuncAuthorize(AppIdentity appInfo, UserIdentity userIdentity, UserAuthOption opt)
    {
        var askFunc = appInfo.ask_auth;
        if (opt.FuncProvider == null)
        {
            if (!string.IsNullOrEmpty(askFunc.func_code))
                throw new NotImplementedException("当前方法设置了权限码，但系统未实现权限码的判断接口！");
            return Resp.DefaultSuccess;
        }

        if (userIdentity.auth_type == PortalAuthorizeType.SuperAdmin)
        {
            userIdentity.data_level = FuncDataLevel.All;
            return Resp.DefaultSuccess;
        }

        var res = await opt.FuncProvider.Authorize(askFunc.func_code);
        if (res.IsSuccess())
            userIdentity.data_level = res.data;

        return res;
    }
}

/// <summary>
///  用户授权参数
/// </summary>
public class UserAuthOption
{
    /// <summary>
    ///  用户授权选项
    /// </summary>
    /// <param name="userProvider"></param>
    /// <exception cref="Exception"></exception>
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