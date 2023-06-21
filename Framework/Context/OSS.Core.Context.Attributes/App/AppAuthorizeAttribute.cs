
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using OSS.Common.Resp;
using OSS.Core.Context.Attributes.Common;

namespace OSS.Core.Context.Attributes;

/// <summary>
///  应用授权过滤器
/// </summary>
public class AppAuthorizeAttribute : BaseOrderAuthorizeAttribute
{
    private readonly AppAuthOption _appOption;

    /// <summary>
    ///  构造函数
    /// </summary>
    public AppAuthorizeAttribute(AppAuthOption appOption)
    {
        _appOption = appOption;
        Order = AttributeConst.Order_App_AuthAttribute;
    }

    /// <summary>
    ///  应用的授权判断处理
    /// </summary>
    public override async Task<Resp> Authorize(AuthorizationFilterContext context)
    {
        // 0.  获取初始化app信息
        if (!CoreContext.App.IsInitialized)
            CoreContext.App.Identity = new AppIdentity();

        var appIdentity = CoreContext.App.Identity;

        // 1. app 内容格式化
        var res = await AppAuthorize(appIdentity, context.HttpContext);
        if (!res.IsSuccess())
            return res;

        CompleteAppIdentity(context.HttpContext, appIdentity);

        return res;
    }

    #region 应用验证


    private async Task<Resp> AppAuthorize(AppIdentity appIdentity, HttpContext context)
    {
        if (appIdentity.auth_mode != AppAuthMode.PartnerContract)
        {
            appIdentity.auth_mode = context.Request.Headers.ContainsKey(_appOption.SignModeTicketHeaderName)
                ? AppAuthMode.AppSign
                : AppAuthMode.None;
        }

        if (context.Request.Headers.TryGetValue("Authorization", out var authValue))
        {
            appIdentity.authorization = authValue.ToString();
        }

        switch (appIdentity.auth_mode)
        {
            case AppAuthMode.AppSign:
                var res = await CheckAppSign(appIdentity, context);
                if (!res.IsSuccess())
                    return res;
                break;

            default:
                appIdentity.app_ver = CoreContext.App.Self.version;
                appIdentity.UDID = "WEB";
                break;
        }

        //  验证要求的类型
        if (appIdentity.auth_mode > appIdentity.ask_meta.app_auth_mode ||
            appIdentity.type > appIdentity.ask_meta.app_type)
        {
            return new Resp(SysRespCodes.NotAllowed, "应用权限不足!");
        }

        return Resp.Success();
    }

    private async Task<Resp> CheckAppSign(AppIdentity appIdentity, HttpContext context)
    {
        string? authTicketStr = context.Request.Headers[_appOption.SignModeTicketHeaderName];
        appIdentity.FormatFromTicket(authTicketStr);

        if (_appOption.SignAccessProvider == null)
            throw new NotImplementedException("请设置应用签名秘钥提供器(SignAccessProvider)");

        var accessRes = await _appOption.SignAccessProvider.GetByKey(appIdentity.access_key);
        if (!accessRes.IsSuccess())
            return accessRes;

        var access = accessRes.data!;
        appIdentity.type = access.type;
        
        return appIdentity.CheckSign(access.access_secret, access.sign_expire_time, appIdentity.authorization);
    }

    // 补充应用全局信息
    private static void CompleteAppIdentity(HttpContext context, AppIdentity sysInfo)
    {
        if (string.IsNullOrEmpty(sysInfo.client_ip))
            sysInfo.client_ip = GetIpAddress(context);

        if (string.IsNullOrEmpty(sysInfo.trace_id))
            sysInfo.trace_id = context.TraceIdentifier = Guid.NewGuid().ToString();
        else
            context.TraceIdentifier = sysInfo.trace_id;

        sysInfo.host = context.Request.Host.ToString();
    }

    // 获取IP地址
    private static string GetIpAddress(HttpContext context)
    {
        string? ipAddress = context.Request.Headers["X-Forwarded-For"];
        return !string.IsNullOrEmpty(ipAddress) ? ipAddress : context.Connection.RemoteIpAddress.ToString();
    }

    #endregion

}

/// <summary>
///  应用授权参数
/// </summary>
public class AppAuthOption
{
    /// <summary>
    ///   应用服务端签名模式，对应的票据信息的请求头名称
    /// </summary>
    public string SignModeTicketHeaderName { get; set; } = "X-Core-Ticket";

    /// <summary>
    ///  签名秘钥提供者
    /// </summary>
    public IAppAccessProvider? SignAccessProvider { get; set; }
}