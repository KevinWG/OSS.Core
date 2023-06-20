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
    private readonly IUserAuthProvider _userProvider;
    private readonly UserAuthOption?   _userOption;

    /// <inheritdoc />
    public UserAuthorizeAttribute(IUserAuthProvider userAuthProvider,UserAuthOption? userOption=null)
    {
        Order         = AttributeConst.Order_User_AuthAttributeOrder;
        _userOption   = userOption;
        _userProvider = userAuthProvider ?? throw new ArgumentNullException(nameof(userAuthProvider),"用户验证实现(UserAuthProvider)没有提供！"); 
    }

    /// <summary>
    ///  授权异步处理
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<Resp> Authorize(AuthorizationFilterContext context)
    {
        if (CoreContext.User.IsAuthenticated)
            return Resp.Success();

        if (context.ActionDescriptor.EndpointMetadata.Any(filter => filter is IAllowAnonymous))
            return Resp.Success();

        var appInfo = CoreContext.App.Identity;

        var userRes = await UserAuthorize(appInfo);
        if (!userRes.IsSuccess())
            return userRes;

        var userIdentity = userRes.data!;
        return await FuncAuthorize(appInfo, userIdentity, _userOption);
    }


    private async Task<Resp<UserIdentity>> UserAuthorize(AppIdentity appIdentity)
    {
        var identityRes = await _userProvider.GetIdentity();
        if (!identityRes.IsSuccess())
            return identityRes;

        var userIdentity = identityRes.data!;
        if (userIdentity.type > appIdentity.ask_meta.user_identity_type)
        {
            switch (userIdentity.type)
            {
                case UserIdentityType.SocialAppUser:
                    return new Resp<UserIdentity>().WithResp(RespCodes.UserFromSocial, "需要绑定系统账号");
                case UserIdentityType.NormalUserWithEmpty:
                    return new Resp<UserIdentity>().WithResp(RespCodes.UserIncomplete, "需要绑定手机号!");
            }

            return new Resp<UserIdentity>().WithResp(RespCodes.UserNoPermission, "权限不足!");
        }

        CoreContext.User.Identity = userIdentity;
        return new Resp<UserIdentity>(userIdentity);
    }


    private static async Task<Resp> FuncAuthorize(AppIdentity appInfo, UserIdentity userIdentity, UserAuthOption? opt)
    {
        var askFunc = appInfo.ask_meta;
        if (opt?.FuncProvider == null)
        {
            if (!string.IsNullOrEmpty(askFunc.func_code))
                throw new NotImplementedException("当前方法设置了权限码，但系统未实现权限码的判断接口！");

            return Resp.Success();
        }

        if (userIdentity.type == UserIdentityType.SuperAdmin)
        {
            userIdentity.data_level = FuncDataLevel.All;
            return Resp.Success();
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
    ///  功能方法权限判断接口
    /// </summary>
    public IFuncAuthProvider? FuncProvider { get; set; }
}