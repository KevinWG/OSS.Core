using OSS.Common.Encrypt;
using OSS.Common.Extension;
using OSS.Common.Helpers;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Portal.Shared.IService;
using OSS.Tools.Config;

namespace OSS.Core.Portal.Service.Common.Helpers;

internal static class PortalTokenHelper
{

    /// <summary>
    ///  获取授权token相关授权信息
    ///    todo 扩展每个AppId独立的加密秘钥
    /// </summary>
    /// <param name="newIdentity"></param>
    /// <returns></returns>
    internal static PortalTokenResp GeneratePortalToken(UserIdentity newIdentity)
    {
        var tenantId = CoreContext.App.Identity.tenant_id;
        var tokenStr = string.Concat(newIdentity.id, "|", tenantId, "|", (int) newIdentity.auth_type, "|",
            NumHelper.RandomNum(6));

        var token = GetToken(tokenStr);
        return new PortalTokenResp {token = token, data = newIdentity};
    }

    internal static Resp<(long userId, PortalAuthorizeType authType)> FormatPortalToken()
    {
        try
        {
            var appInfo = CoreContext.App.Identity;
            if (string.IsNullOrEmpty(appInfo.token))
                return new Resp<(long, PortalAuthorizeType)>().WithResp(RespTypes.UserUnLogin, "用户未登录");

            var tokenDetail = GetTokenDetail(appInfo.token);
            var tokenSplit  = tokenDetail.Split('|');
            if (tokenSplit.Length != 4)
                return new Resp<(long, PortalAuthorizeType)>().WithResp(RespTypes.OperateFailed,
                    "非合法授权来源!");

            var tenantId = tokenSplit[1];
            var userId   = tokenSplit[0].ToInt64();

            if (!string.IsNullOrEmpty(appInfo.tenant_id) && tenantId != appInfo.tenant_id
                || userId <= 0)
                return new Resp<(long, PortalAuthorizeType)>().WithResp(RespTypes.OperateFailed,
                    "非合法授权来源!");

            var authType = (PortalAuthorizeType) tokenSplit[2].ToInt32();
            return new Resp<(long, PortalAuthorizeType)>()
            {
                data = (userId, authType)
            };
        }
        catch
        {
            return new Resp<(long, PortalAuthorizeType)>().WithResp(RespTypes.UserUnLogin, "用户未登录");
        }
    }

    #region 辅助方法  token  处理

    private static string GetToken(string tokenDetail)
    {
        var key = GetPortalTokenSecret();
        return AesRijndael.Encrypt(tokenDetail, key).ReplaceBase64ToUrlSafe();
    }

    private static string GetTokenDetail(string token)
    {
        var key         = GetPortalTokenSecret();
        var tokenDetail = AesRijndael.Decrypt(token.ReplaceBase64UrlSafeBack(), key);

        if (string.IsNullOrEmpty(tokenDetail))
            throw new RespException(RespTypes.ParaError, "不合法的用户Token");

        return tokenDetail;
    }

    private static readonly string? PortalTokenSecret =
        ConfigHelper.GetSection("SiteConfig:PortalTokenSecret")?.Value;

    private static string GetPortalTokenSecret()
    {
        if (string.IsNullOrEmpty(PortalTokenSecret))
        {
            throw new RespOperateErrorException("请联系管理员配置SiteConfig:PortalTokenSecret 节点信息！");
        }

        return PortalTokenSecret;
    }

    #endregion
}