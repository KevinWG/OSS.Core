using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes.Common;

namespace OSS.Core.Context.Attributes;

/// <summary>
///  功能权限名称过滤器
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class UserFuncMetaAttribute : BaseOrderAuthorizeAttribute
{
    private readonly string _funcCode;

    /// <summary>
    /// 业务请求中场景值对应的参数
    ///  设置后将从请求参数中获取对应的参数值，记录在上下文的 scene_code 中，方便权限验证处理
    /// </summary>
    public string scene_query_name { get; set; } = string.Empty;

    /// <summary>
    ///   要求的授权类型，默认为管理员类型
    /// </summary>
    public UserIdentityType user_identity_type { get; set; }

    /// <summary>
    /// 功能权限验证
    /// </summary>
    /// <param name="idType"> 要求的授权类型</param>
    public UserFuncMetaAttribute(UserIdentityType idType)
        : this(string.Empty, idType)
    {
    }

    /// <summary>
    /// 功能权限验证
    ///     默认验证是否管理员
    /// </summary>
    public UserFuncMetaAttribute() : this(string.Empty, UserIdentityType.Admin)
    {
    }

    /// <summary>
    /// 功能权限验证
    /// </summary>
    /// <param name="funcCode"></param>
    /// <param name="idType">  要求的授权类型，默认为管理员类型 </param>
    public UserFuncMetaAttribute(string funcCode, UserIdentityType idType = UserIdentityType.Admin)
    {
        Order = AttributeConst.Order_User_FuncMetaAttribute;

        _funcCode = funcCode;
        user_identity_type = idType;
    }

    /// <inheritdoc />
    public override Task<Resp> Authorize(AuthorizationFilterContext context)
    {
        var sceneCode = string.Empty;
        var appInfo = CoreContext.App.Identity;

        if (!string.IsNullOrEmpty(scene_query_name))
            sceneCode = context.HttpContext.Request.Query[scene_query_name].ToString();

        appInfo.ask_auth.user_identity_type = user_identity_type;
        appInfo.ask_auth.func_code = string.Concat(_funcCode, sceneCode);

        return Task.FromResult(Resp.Success());
    }

}