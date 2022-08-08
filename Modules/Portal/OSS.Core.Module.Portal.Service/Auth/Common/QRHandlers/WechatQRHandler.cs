using OSS.Clients.Msg.Wechat;
using OSS.Clients.Platform.Wechat;
using OSS.Clients.Platform.Wechat.QR;
using OSS.Clients.Platform.Wechat.User;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Domain;
using OSS.DataFlow;
using OSS.Tools.Log;

namespace OSS.Core.Module.Portal;

/// <summary>
///  微信公众号扫码登录
/// </summary>
internal class WechatOfficialQRHandler
{
    internal static void StartSubscriber()
    {
        DataFlowFactory.RegisterSubscriber<WechatSubScanRecEventMsg>(PortalConst.DataFlowMsgKeys.Chat_Wechat_SubScan,
            SubScanUpdate);
    }
    
    /// <summary>
    /// 获取微信的二维码
    /// </summary>
    /// <param name="platform"></param>
    /// <param name="portalCode"></param>
    /// <returns></returns>
    public async Task<StrResp> GetQRCodeUrl(PortalType platform, string portalCode)
    {
        //var appConfigRes = await InsContainer<IAppCenterClient>.Instance.AppService.GetAppById(scene);
        var wechatQRCodeResp = await new WechatQRCodeReq(new WechatQrCodeBody()
            {
                action_info = new WechatSenceQrMo()
                {
                    scene = new WechatSenceQrInfoMo()
                    {
                        scene_str = string.Concat("portal", portalCode)
                    }
                },

                action_name    = WechatQrCodeType.QR_STR_SCENE.ToString(),
                expire_seconds = 60 * 60 // 一个小时过期
            })
            // todo 修改通过应用中心获取配置信息
            //.SetContextConfig(appConfigRes.data)
            
            .SendAsync();

        return wechatQRCodeResp.IsSuccess()
            ? new StrResp(wechatQRCodeResp.url)
            : new StrResp().WithResp(wechatQRCodeResp);
    }

    private static async Task<bool> SubScanUpdate(WechatSubScanRecEventMsg msg)
    {
        try
        {
            await ProcessPortal(msg);
            return true;
        }
        catch (Exception e)
        {
            LogHelper.Error($"微信二维码登录异常({e.Message}),详情：{e.StackTrace}", nameof(WechatOfficialQRHandler));
        }
        return false;
    }

    //  处理登录二维码
    private static async Task ProcessPortal(WechatSubScanRecEventMsg msg)
    {
        var eventKey = msg.EventKey;

        // 判断登录二维码前缀
        if (string.IsNullOrEmpty(eventKey) || !(eventKey.StartsWith("portal") || eventKey.StartsWith("qrscene_portal")))
            return;

        // 获取登录编码
        var scenePortalCode = eventKey.StartsWith("portal") ? eventKey.Substring(6) : eventKey.Substring(14);
        if (string.IsNullOrEmpty(scenePortalCode))
            return;

        await PortalSocialHelper.SetPortalCodeUserLink(scenePortalCode, new QRCodeLinkUserResp()
        {
            progress = QRProgress.Processing
        });

        var socialUserRes = await AddOrUpdateSocialUser(msg);
        var codeUserRes = new QRCodeLinkUserResp().WithResp(socialUserRes, (suRes, cRes) =>
        {
            cRes.progress    = QRProgress.ProcessEnd;
            cRes.app_user_id = suRes.data;
            cRes.platform    = AppPlatform.Wechat;
        });

        await PortalSocialHelper.SetPortalCodeUserLink(scenePortalCode, codeUserRes);
    }

    private static async Task<LongResp> AddOrUpdateSocialUser(WechatSubScanRecEventMsg msg)
    {
        try
        {
            var openId      = msg.FromUserName;
            var wechatAppKey = msg.AppId;
            
            var wechatUserRes = await new WechatUserInfoReq(openId).SendAsync();
            if (!wechatUserRes.IsSuccess())
                return new LongResp().WithResp(wechatUserRes);

            var socialUser = wechatUserRes.ToSocialUserReq(wechatAppKey);

            return await InsContainer<ISocialUserService>.Instance.AddOrUpdateSocialUser(socialUser);
        }
        catch (Exception e)
        {
            LogHelper.Error($"获取更新微信公众号用户信息失败（{e.Message}），详情:{e.StackTrace}", nameof(WechatOfficialQRHandler));
        }

        return new LongResp().WithResp(SysRespCodes.AppError, "获取更新微信用户信息失败!");
    }
}

