//#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

///***************************************************************************
//*　　	文件功能描述：OSSCore服务层 —— 登录注册入口 service （前后台用户信息
//*
//*　　	创建人： Kevin
//*       创建人Email：1985088337@qq.com
//*    	创建日期：2017-5-17
//*       
//*****************************************************************************/

//#endregion

//using OSS.Clients.MApp.Wechat;
//using OSS.Clients.Platform.Wechat;
//using OSS.Common;
//using OSS.Common.Extension;
//using OSS.Common.Resp;
//using OSS.Core.Context;
//using OSS.Core.Domain;
//using OSS.Core.Module.Portal.Domain;
//using OSS.Core.Services.Basic.Portal.Reqs;

//namespace OSS.Core.Module.Portal.Service
//{
//    /// <summary>
//    /// 应用小程序登录授权
//    /// </summary>
//    public class MAppAuthService : BaseSocialAuthService
//    {
//        #region 微信 获取小程序授权登录

//        public async Task<PortalTokenResp> WechatAuthSession(string wechatAppId, string code)
//        {
//            var appConfigRes = await InsContainer<IAppService>.Instance.GetAppByAppId(AppPlatform.Wechat, wechatAppId);
//            var wechatRes = await new WechatSessionReq(code)
//                .SetContextConfig(appConfigRes.data)
//                .SendAsync();

//            if (!wechatRes.IsSuccess())
//                return new PortalTokenResp().WithResp(wechatRes);

//            var socialUserRes = await AddOrUpdateMAppUserSession(wechatRes, appConfigRes.data.app_id);
//            if (!socialUserRes.IsSuccess())
//                return new PortalTokenResp().WithResp(socialUserRes);

//            return await LoginBySocialUser(socialUserRes.data, false);
//        }

//        public async Task<IResp> WechatUserUpdate(WechatMAppEncryptBody body)
//        {
//            if (string.IsNullOrEmpty(body.encrypt_data) || string.IsNullOrEmpty(body.iv))
//                return new Resp(RespCodes.ParaError, "参数异常错误!");

//            var socialUserRes = await SocialUserRep.Instance.GetById(CoreUserContext.Identity.id.ToInt64());
//            if (!socialUserRes.IsSuccess())
//                return socialUserRes;

//            var socialUser = socialUserRes.data;
//            var wechatUser = body.ToUserInfo(socialUser.access_token);
//            if (wechatUser == null)
//                return new Resp(RespCodes.ParaError, "微信小程序加密数据信息异常!");

//            socialUser.FormatByWechatMappUser(wechatUser);

//            return await SocialUserRep.Instance.UpdateSocialUser(socialUser);
//        }

//        /// <summary>
//        ///  通过微信小程序获取手机号代替绑定页，完成第三方用户和系统用户绑定(由手机号直接注册或登录)
//        /// 【已经是第三方账号临时登录状态】
//        /// </summary>
//        /// <param name="body"></param>
//        /// <returns></returns>
//        public Task<PortalTokenResp> WechatPhoneRegLogin(WechatMAppEncryptBody body)
//        {
//            return WechatPhoneRegLogin(body, false);
//        }

//        /// <summary>
//        ///  通过微信小程序获取手机号代替绑定页，完成第三方用户和系统用户绑定(由手机号直接注册或登录)
//        /// 【已经是第三方账号临时登录状态】
//        /// </summary>
//        /// <param name="body"></param>
//        /// <returns></returns>
//        private async Task<PortalTokenResp> WechatPhoneRegLogin(WechatMAppEncryptBody body, bool isAdmin)
//        {
//            if (string.IsNullOrEmpty(body.encrypt_data) || string.IsNullOrEmpty(body.iv))
//                return new PortalTokenResp().WithResp(RespCodes.ParaError, "参数异常错误!");

//            var socialUserRes = await SocialUserRep.Instance.GetById(CoreUserContext.Identity.id.ToInt64());
//            if (!socialUserRes.IsSuccess())
//                return new PortalTokenResp().WithResp(socialUserRes);

//            var socialUser = socialUserRes.data;
//            var wechatPhoneInfo = body.ToUserPhone(socialUser.access_token);

//            var userRes =
//                await UserInfoRep.Instance.GetUserByLoginType(wechatPhoneInfo.purePhoneNumber, PortalCodeType.Mobile);
//            if (!userRes.IsSuccess() && !userRes.IsRespCode(RespCodes.OperateObjectNull))
//                return new PortalTokenResp().WithResp(userRes);

//            if (userRes.IsSuccess())
//            {
//                // 不管当前第三方账号有没有绑定过，统一绑定到 手机对应的账号 下
//                return await LoginFinallyExecute(userRes.data, isAdmin, 1);
//            }

//            if (isAdmin)
//                return new PortalTokenResp().WithResp(RespCodes.OperateFailed, "非管理员用户!");

//            var user = socialUser.ToUserInfo();
//            user.mobile = wechatPhoneInfo.purePhoneNumber;

//            return await RegFinallyExecute(user, 1);
//        }

//        private static async Task<Resp<SocialUserMo>> AddOrUpdateMAppUserSession(WechatSessionResp wechatRes, string socialAppId)
//        {
//            var socialUserRes = await SocialUserRep.Instance.GetByAppUserId(wechatRes.openid, socialAppId, AppPlatform.Wechat);
//            if (socialUserRes.IsSuccess())
//            {
//                //  如果存在，更新已有信息
//                var sUser = socialUserRes.data;

//                sUser.access_token = wechatRes.session_key;
//                sUser.app_union_id = wechatRes.unionid;

//                await SocialUserRep.Instance.UpdateWechatMAppSession(sUser.id, sUser.access_token, sUser.app_union_id);

//                return new Resp<SocialUserMo>(sUser);
//            }

//            //  其他错误，直接返回
//            if (!socialUserRes.IsRespCode(RespCodes.OperateObjectNull))
//                return socialUserRes;

//            var socialUser = new SocialUserMo
//            {
//                access_token = wechatRes.session_key,
//                app_user_id = wechatRes.openid,
//                app_union_id = wechatRes.unionid,
//                social_plat = AppPlatform.Wechat,
//                social_app_id = socialAppId
//            };
//            socialUser.InitialBaseFromContext();

//            var idRes = await SocialUserRep.Instance.Add(socialUser);
//            if (!idRes.IsSuccess())
//                return new Resp<SocialUserMo>().WithResp(idRes.ret,
//                    "添加授权用户信息失败！"); // idRes.ConvertToResult<OauthUserMo>();

//            return new Resp<SocialUserMo>(socialUser);
//        }

//        #endregion
//    }
//}