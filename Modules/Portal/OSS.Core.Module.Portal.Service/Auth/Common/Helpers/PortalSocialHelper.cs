using OSS.Common.Encrypt;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Domain;
using OSS.Tools.Cache;

namespace OSS.Core.Module.Portal
{
    internal static class PortalSocialHelper
    {

        public static string GetPortalTicketByCode(string portalCode)
        {
            var str = string.Concat(portalCode, "|", DateTime.Now.ToUtcSeconds());
            return AesRijndael.Encrypt(str, PortalTokenHelper.UserTokenSecret).ReplaceBase64ToUrlSafe();
        }

        public static StrResp GetPortalCodeByTicket(string portalTicket)
        {
            if (string.IsNullOrEmpty(portalTicket))
                return new StrResp().WithResp(RespCodes.ParaError, "无效的票据信息!");

            var ticketNormal = portalTicket.ReplaceBase64UrlSafeBack();
            var ticketBody = AesRijndael.Decrypt(ticketNormal, PortalTokenHelper.UserTokenSecret);
            if (string.IsNullOrEmpty(ticketBody))
                return new StrResp().WithResp(RespCodes.ParaError, "非法的登录请求!");

            var bodyStrs = ticketBody.Split('|');
            if (bodyStrs.Length != 2)
                return new StrResp().WithResp(RespCodes.ParaError, "非法的登录请求!");

            //var time = bodyStrs[1].ToInt64();
            //if (time+60*2<DateTime.Now.ToUtcSeconds())
            //    return new StrResp().WithResp(RespCodes.ParaExpired, "请求参数内容已过期!");

            return new StrResp(bodyStrs[0]);
        }


        #region 二维码Code用户关联缓存处理

        /// <summary>
        ///  设置二维码登录编码对应的登录信息
        /// </summary>
        /// <param name="portalCode"></param>
        /// <param name="qrCodeUser"></param>
        /// <returns></returns>
        public static Task<bool> SetPortalCodeUserLink(string portalCode, QRCodeLinkUserResp qrCodeUser)
        {
            var cacheKey = string.Concat(PortalConst.CacheKeys.Portal_UserId_ByQRCode, portalCode);
            return CacheHelper.SetAbsoluteAsync(cacheKey, qrCodeUser, TimeSpan.FromMinutes(3));
        }

        /// <summary>
        ///  获取二维码对应的用户
        /// </summary>
        /// <param name="portalCode"></param>
        /// <returns></returns>
        public static Task<QRCodeLinkUserResp> GetPortalCodeUserLink(string portalCode)
        {
            var cacheKey = string.Concat(PortalConst.CacheKeys.Portal_UserId_ByQRCode, portalCode);
            return CacheHelper.GetAsync<QRCodeLinkUserResp>(cacheKey);
        }

        #endregion
    }

    public class QRCodeLinkUserResp : Resp
    {
        /// <summary>
        ///  平台信息
        /// </summary>
        public AppPlatform platform { get; set; }

        /// <summary>
        ///   如果是外部平台对应的为 SocialUserId
        ///   如果是系统自身对应的为 UserId
        /// </summary>
        public long app_user_id { get; set; }

        /// <summary>
        /// 二维码处理进度
        /// </summary>
        public QRProgress progress { get; set; }
    }
}
