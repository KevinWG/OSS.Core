using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.Portal
{
    /// <summary>
    ///  应用扫码登录授权
    /// </summary>
    public class QRAuthService : BaseSocialAuthService, IQRAuthOpenService
    {
        #region 获取二维码

        /// <summary>
        ///  获取二维码登录的内容
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public async Task<GetPortalQRResp> GetQRCode(PortalType platform)
        {
            //if (!AppInfoHelper.IsProduct)
            //    return new GetPortalQRResp().WithResp(RespCodes.OperateFailed, "非生产环境不可以执行!");

            var portalCode = NumHelper.SnowNum().ToCode();

            var qrCodeRes = await GetAppQRCode(platform, portalCode);
            if (!qrCodeRes.IsSuccess())
                return new GetPortalQRResp().WithResp(qrCodeRes);

            return new GetPortalQRResp()
            {
                portal_ticket = PortalSocialHelper.GetPortalTicketByCode(portalCode),
                qr_code       = qrCodeRes.data
            };
        }

        // 扩展其他平台的支持
        private static Task<StrResp> GetAppQRCode(PortalType platform, string portalCode)
        {
            // 暂时仅有微信公众号
            return SingleInstance<WechatOfficialQRHandler>.Instance.GetQRCodeUrl(platform, portalCode);
        }

        #endregion

        #region 获取扫码结果

        /// <summary>
        ///   查询用户登录状态并返回登录信息
        /// </summary>
        /// <param name="portalTicket"></param>
        /// <returns></returns>
        public Task<PortalQRTokenResp> AskProgress(string portalTicket)
        {
            return GetPortalProgress(portalTicket, false);
        }

        /// <summary>
        ///   查询管理员登录状态并返回登录信息
        /// </summary>
        /// <param name="portalTicket"></param>
        /// <returns></returns>
        public Task<PortalQRTokenResp> AskAdminProgress(string portalTicket)
        {
            return GetPortalProgress(portalTicket, true);
        }

        /// <summary>
        ///  获取扫码后登录状态
        /// </summary>
        /// <param name="portalTicket"></param>
        /// <returns></returns>
        private async Task<PortalQRTokenResp> GetPortalProgress(string portalTicket, bool isAdmin)
        {
            var pCodeRes = PortalSocialHelper.GetPortalCodeByTicket(portalTicket);
            if (!pCodeRes.IsSuccess())
                return new PortalQRTokenResp().WithResp(pCodeRes);

            var portalUserLinkRes = await PortalSocialHelper.GetPortalCodeUserLink(pCodeRes.data);
            if (portalUserLinkRes == null)
                return new PortalQRTokenResp() {progress = QRProgress.WaitScan};

            // 失败
            if (!portalUserLinkRes.IsSuccess())
                return new PortalQRTokenResp().WithResp(portalUserLinkRes);

            // 回调处理中
            if (portalUserLinkRes.progress == QRProgress.Processing)
                return new PortalQRTokenResp() {progress = portalUserLinkRes.progress};

            //  回调处理关联用户成功，执行登录
            if (portalUserLinkRes.platform == AppPlatform.Self)
            {
                var userRes = await  m_AuthRep.GetById(portalUserLinkRes.app_user_id);
                if (!userRes.IsSuccess())
                    return new PortalQRTokenResp().WithResp(RespCodes.OperateFailed, "无效用户信息！");

                return (await LoginFinallyExecute(userRes.data, isAdmin)).ToQRToken();
            }

            var socialUserRes = await m_SocialRep.GetById(portalUserLinkRes.app_user_id);
            if (!socialUserRes.IsSuccess())
                return new PortalQRTokenResp().WithResp(RespCodes.OperateFailed, "无效用户信息！");

            return (await RegLoginBySocialUser(socialUserRes.data, isAdmin)).ToQRToken();
        }

        #endregion
    }
}
