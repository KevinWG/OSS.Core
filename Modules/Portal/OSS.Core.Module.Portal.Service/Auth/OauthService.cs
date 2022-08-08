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

//using OSS.Common.Resp;
//using OSS.Core.Domain;
//using OSS.Core.Module.Portal.Domain;
//using OSS.Core.Services.Basic.Portal.Reqs;

//namespace OSS.Core.Module.Portal.Service
//{
//    /// <summary>
//    ///  应用Oauth登录授权
//    /// </summary>
//    public class OauthService : BaseSocialAuthService
//    {
//        #region 获取第三方Oauth授权地址

//        /// <summary>
//        ///  获取oauth授权地址
//        /// todo 扩展 socialAppid
//        /// </summary>
//        /// <param name="plat"></param>
//        /// <param name="redirectUrl"></param>
//        /// <param name="state"></param>
//        /// <param name="type"></param>
//        /// <returns></returns>
//        public Task<StrResp> GetOauthUrl(AppPlatform plat, string redirectUrl, string state, OauthClientType type)
//        {
//            return GetOauthAdapter(plat).GetOauthUrl(redirectUrl, state, type);
//        }

//        #endregion
        
//        #region 【未登录】用户通过第三方账号登录/注册

//        /// <summary>
//        ///  通过第三方账号注册登录
//        ///     如果没有绑定过用户，会根据系统配置，决定是否直接创建默认账户.
//        ///     根据返回的auth_type,前端决定是否进入手动绑定账户页面
//        /// </summary>
//        /// <param name="plat"></param>
//        /// <param name="code"></param>
//        /// <param name="state"></param>
//        /// <returns></returns>
//        public Task<PortalTokenResp> OauthRegLogin(AppPlatform plat,string socialAppId, string code, string state)
//        {
//            return OauthRegLogin(plat, socialAppId, code, state, false);
//        }

//        /// <summary>
//        ///     通过第三方用户登录管理员账号
//        /// </summary>
//        /// <param name="plat"></param>
//        /// <param name="code"></param>
//        /// <param name="state"></param>
//        /// <returns></returns>
//        public Task<PortalTokenResp> OauthAdminLogin(AppPlatform plat, string socialAppId, string code, string state)
//        {
//            return OauthRegLogin(plat, socialAppId, code, state, true);
//        }

//        #endregion

//        #region 辅助方法

//        /// <summary>
//        ///  根据第三方信息处理登录或注册
//        ///     （如果未绑定过系统账号，根据配置处理是否默认注册绑定
//        /// </summary>
//        /// <param name="plat"></param>
//        /// <param name="code"></param>
//        /// <param name="state"></param>
//        /// <param name="authType"></param>
//        /// <returns></returns>
//        private async Task<PortalTokenResp> OauthRegLogin(AppPlatform plat,string socialAppId, string code, string state, bool is_admin)
//        {
//            // 先获取第三方账号最新信息，更新至本地
//            var socialUserRes = await AddOrUpdateOauthUser(plat, socialAppId, code, state);
//            if (!socialUserRes.IsSuccess())
//                return new PortalTokenResp().WithResp(socialUserRes);

//            return await RegLoginBySocialUser(socialUserRes.data, is_admin);
//        }

//        /// <summary>
//        ///   添加或更新第三方授权用户信息
//        /// </summary>
//        /// <param name="plat"></param>
//        /// <param name="code"></param>
//        /// <param name="state"></param>
//        /// <returns></returns>
//        private static async Task<Resp<SocialUserMo>> AddOrUpdateOauthUser(AppPlatform plat,string socialAppId, string code, string state)
//        {
//            var userWxRes = await GetOauthUserByCode(plat, code, state);
//            if (!userWxRes.IsSuccess())
//                return new Resp<SocialUserMo>().WithResp(userWxRes);

//            // 检查已有平台账号
//            var socialUserRes = await SocialUserRep.Instance.GetByAppUserId(userWxRes.data.app_user_id, socialAppId, plat);
//            if (socialUserRes.IsSuccess())
//            {
//                //  如果存在，更新已有信息
//                var user = socialUserRes.data;

//                user.FormatByOauthUser(userWxRes.data);

//                await SocialUserRep.Instance.UpdateSocialUser(user);
//                return new Resp<SocialUserMo>(user);
//            }

//            //  其他错误，直接返回
//            if (!socialUserRes.IsRespCode(RespCodes.OperateObjectNull))
//                return socialUserRes;

//            // 如果是新授权用户，添加新的信息
//            var newSocialUser = userWxRes.data.ToSocialUser(socialAppId);
//            var idRes         = await SocialUserRep.Instance.Add(newSocialUser);
//            return idRes.IsSuccess()
//                ? new Resp<SocialUserMo>(newSocialUser)
//                : new Resp<SocialUserMo>().WithResp(idRes.ret, "添加授权用户信息失败！");
//        }

//        /// <summary>
//        ///   获取授权用户并更新信息
//        ///  todo 添加Appid
//        /// </summary>
//        /// <param name="plat"></param>
//        /// <param name="code"></param>
//        /// <param name="state"></param>
//        /// <returns></returns>
//        private static async Task<Resp<OauthUser>> GetOauthUserByCode(AppPlatform plat, string code, string state)
//        {
//            var handler = GetOauthAdapter(plat);
//            var tokenRes = await handler.GetOauthTokenAsync(code, state);
//            if (!tokenRes.IsSuccess())
//                return new Resp<OauthUser>().WithResp(tokenRes);

//            var userWxRes = await handler.GetOauthUserAsync(tokenRes.data.access_token, tokenRes.data.app_user_id);
//            if (!userWxRes.IsSuccess())
//                return new Resp<OauthUser>().WithResp(tokenRes);

//            userWxRes.data.SetTokenInfo(tokenRes.data);
//            return userWxRes;
//        }

//        private static IOauthAdapter GetOauthAdapter(AppPlatform plat)
//        {
//            return OauthAdapterHub.GetAdapter(plat.ToOauthPlat());
//        }

//        #endregion
        
//        #region SocialUser临时登录


//        ///// <summary>
//        /////   对于进入手动绑定账户页面的用户，如果选择跳过，则执行此方法，创建默认账户
//        ///// 【当前MemberContext信息为 SocialUser】
//        ///// </summary>
//        ///// <returns></returns>
//        //public async Task<PortalTokenResp> SkipWithReg()
//        //{
//        //    var tempOauthRes = PortalHelper.FormatPortalToken();
//        //    if (!tempOauthRes.IsSuccess())
//        //        return new PortalTokenResp().WithResp(tempOauthRes);

//        //    if (tempOauthRes.data.authType != PortalAuthorizeType.SocialAppUser)
//        //        return new PortalTokenResp() { ret = (int)RespCodes.OperateObjectNull, msg = "未能找到第三方信息！" };

//        //    var oauthUserRes = await SocialUserRep.Instance.GetById(tempOauthRes.data.userId);
//        //    if (!oauthUserRes.IsSuccess())
//        //        return new PortalTokenResp() { ret = (int)RespCodes.OperateObjectNull, msg = "未能找到第三方信息！" };

//        //    return await OauthReg(oauthUserRes.data);
//        //}



//        #endregion
//    }
//}