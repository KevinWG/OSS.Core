using OSS.Common;
using OSS.Common.Encrypt;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Tools.Config;

namespace OSS.Core.Module.Portal;

internal static class PortalTokenHelper
{
    /// <summary>
    ///  应用内部Token加密秘钥
    /// </summary>
    internal static readonly string UserTokenSecret;

    static PortalTokenHelper()
    {
        UserTokenSecret = ConfigHelper.GetSection("Module:Portal:UserTokenSecret")?.Value ?? string.Empty;
        if (string.IsNullOrEmpty(UserTokenSecret))
            throw new NotImplementedException("未能找到用户Token加密秘钥（ Module:Portal:UserTokenSecret 节点）");
    }
    
    /// <summary>
    ///  获取授权token相关授权信息
    /// </summary>
    /// <param name="newIdentity"></param>
    /// <returns></returns>
    internal static PortalTokenResp GeneratePortalToken(UserIdentity newIdentity)
    {
        var tokenStr = string.Concat(newIdentity.id, "|", (int) newIdentity.auth_type, "|",
            NumHelper.RandomNum(6));

        var token = GetToken(UserTokenSecret, tokenStr);
        return new PortalTokenResp {token = token, data = newIdentity};
    }

    internal static Resp<(long userId, PortalAuthorizeType authType)> FormatPortalToken()
    {
        try
        {
            var appInfo = CoreContext.App.Identity;
            if (string.IsNullOrEmpty(appInfo.token))
                return new Resp<(long, PortalAuthorizeType)>().WithResp(RespCodes.UserUnLogin, "用户未登录");

            var tokenDetail = GetTokenDetail(UserTokenSecret, appInfo.token);
            var tokenSplit  = tokenDetail.Split('|');

            if (tokenSplit.Length != 3)
                return new Resp<(long, PortalAuthorizeType)>().WithResp(RespCodes.OperateFailed,
                    "非合法授权来源!");

            var userId = tokenSplit[0].ToInt64();
            if (userId <= 0)
                return new Resp<(long, PortalAuthorizeType)>().WithResp(RespCodes.OperateFailed, "非合法授权来源!");

            var authType = (PortalAuthorizeType) tokenSplit[1].ToInt32();
            return new Resp<(long, PortalAuthorizeType)>()
            {
                data = (userId, authType)
            };
        }
        catch
        {
            return new Resp<(long, PortalAuthorizeType)>().WithResp(RespCodes.UserUnLogin, "用户未登录");
        }
    }



    /// <summary>
    /// 通过 ID 生成对应的Token
    /// </summary>
    /// <param name="encryptKey"></param>
    /// <param name="tokenDetail"></param>
    /// <returns></returns>
    public static string GetToken(string encryptKey, string tokenDetail)
    {
        return AesRijndael.Encrypt(tokenDetail, encryptKey).ReplaceBase64ToUrlSafe();
    }

    /// <summary>
    ///  通过token解析出对应的id和key
    /// </summary>
    /// <param name="encryptKey"></param>
    /// <param name="token"></param>
    /// <returns>返回解析信息，Item1为id，Item2为key</returns>
    public static string GetTokenDetail(string encryptKey, string token)
    {
        var tokenDetail = AesRijndael.Decrypt(token.ReplaceBase64UrlSafeBack(), encryptKey);

        if (string.IsNullOrEmpty(tokenDetail))
            throw new ArgumentNullException(nameof(token), "不合法的用户Token");

        return tokenDetail;
    }
}