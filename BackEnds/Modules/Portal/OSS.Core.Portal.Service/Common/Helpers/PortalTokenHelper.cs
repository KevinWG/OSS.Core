using OSS.Common.Resp;
using OSS.Common.Extension;
using OSS.Common.Helpers;
using OSS.Core.Context;
using OSS.Tools.Config;
using OSS.Core.Services.Basic.Portal.Reqs;

namespace OSS.Core.Services.Basic.Portal.Helpers
{
    static internal class PortalTokenHelper
    {
        /// <summary>
        ///  应用内部Token加密秘钥
        /// </summary>
        internal static readonly string PortalTokenSecret = ConfigHelper.GetSection("SiteConfig:PortalTokenSecret")?.Value;

        /// <summary>
        ///  获取授权token相关授权信息
        ///    todo 扩展每个AppId独立的加密秘钥
        /// </summary>
        /// <param name="newIdentity"></param>
        /// <param name="plat"></param>
        /// <returns></returns>
        internal static PortalTokenResp GeneratePortalToken(UserIdentity newIdentity)
        {
            var tenantId = CoreAppContext.Identity.tenant_id;
            var tokenStr = string.Concat(newIdentity.id, "|", tenantId, "|", (int)newIdentity.auth_type, "|", NumHelper.RandomNum(6));

            var token = CoreUserContext.GetToken(PortalTokenSecret, tokenStr);
            return new PortalTokenResp { token = token, data = newIdentity };
        }

        internal static Resp<(long userId, PortalAuthorizeType authType)> FormatPortalToken()
        {
            try
            {
                var appInfo = CoreAppContext.Identity;
                if (string.IsNullOrEmpty(appInfo.token))
                    return new Resp<(long, PortalAuthorizeType)>().WithResp(RespTypes.UserUnLogin, "用户未登录");

                var tokenDetail = CoreUserContext.GetTokenDetail(PortalTokenSecret, appInfo.token);
                var tokenSplit = tokenDetail.Split('|');
                if (tokenSplit.Length != 4)
                    return new Resp<(long, PortalAuthorizeType)>().WithResp(RespTypes.OperateFailed,
                        "非合法授权来源!");

                var tenantId = tokenSplit[1];
                var userId = tokenSplit[0].ToInt64();

                if (!string.IsNullOrEmpty(appInfo.tenant_id) && tenantId != appInfo.tenant_id
                    || userId <= 0)
                    return new Resp<(long, PortalAuthorizeType)>().WithResp(RespTypes.OperateFailed,
                        "非合法授权来源!");

                var authType = (PortalAuthorizeType)tokenSplit[2].ToInt32();
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
    }
}