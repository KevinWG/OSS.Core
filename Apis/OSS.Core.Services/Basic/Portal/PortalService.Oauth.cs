#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 登录注册入口 service （前后台用户信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-17
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.Adapter.Oauth.Interface;
using OSS.Adapter.Oauth.Interface.Mos;
using OSS.Adapter.Oauth.Interface.Mos.Enums;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Extension;
using OSS.Core.Context;
using OSS.Core.RepDapper.Basic.Portal;
using OSS.Core.RepDapper.Basic.Portal.Mos;
using OSS.Core.RepDapper.Basic.SocialPlats.Mos;
using OSS.Core.Services.Basic.Portal.Events;
using OSS.Core.Services.Basic.Portal.Helpers;
using OSS.Core.Services.Basic.Portal.Mos;

namespace OSS.Core.Services.Basic.Portal
{
    public partial class PortalService
    {
        #region 获取第三方Oauth授权地址

        /// <summary>
        ///  获取oauth授权地址
        /// </summary>
        /// <param name="plat"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="state"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Task<StrResp> GetOauthUrl(SocialPlatform plat, string redirectUrl, string state, OauthClientType type)
        {
            return GetOauthAdapter(plat).GetOauthUrl(redirectUrl, state, type);
        }
        
        #endregion

        #region 【已登录】用户直接绑定第三方账号

        /// <summary>
        ///  系统用户主动绑定第三方平台
        /// 【当前MemberContext信息为 系统用户信息(User  or  admin)】
        /// </summary>
        /// <param name="plat"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<Resp> OauthBind(SocialPlatform plat, string code, string state)
        {
            var oauthUserRes = await AddOrUpdateOauthUser(plat, code, state);
            if (!oauthUserRes.IsSuccess())
                return oauthUserRes;

            var identityRes = await GetMyself();
            if (!identityRes.IsSuccess())
                return identityRes;

            return await OauthUserRep.Instance.BindUserIdByOauthId(oauthUserRes.data.id, identityRes.data.id.ToInt64());
        }

        #endregion

        #region 【未登录】用户通过第三方账号登录/注册

        /// <summary>
        ///  通过第三方账号注册登录
        ///     如果没有绑定过用户，会根据系统配置，决定是否直接创建默认账户.
        ///     根据返回的auth_type,前端决定是否进入手动绑定账户页面
        /// </summary>
        /// <param name="plat"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public Task<PortalTokenResp> OauthLogin(SocialPlatform plat, string code, string state)
        {
            return OauthRegLogin(plat, code, state, PortalAuthorizeType.User);
        }

        /// <summary>
        ///     通过第三方用户登录管理员账号
        /// </summary>
        /// <param name="plat"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public Task<PortalTokenResp> OauthAdminLogin(SocialPlatform plat, string code, string state)
        {
            return OauthRegLogin(plat, code, state, PortalAuthorizeType.Admin);
        }

        /// <summary>
        ///   对于进入手动绑定账户页面的用户，如果选择跳过，则执行此方法，创建默认账户
        /// </summary>
        /// <returns></returns>
        public async Task<PortalTokenResp> SkipWithReg()
        {
            var tempOauthRes = FormatPortalToken();
            if (!tempOauthRes.IsSuccess())
                return new PortalTokenResp().WithResp(tempOauthRes);

            if (tempOauthRes.data.authType != PortalAuthorizeType.SocialAppUser)
                return new PortalTokenResp() { ret = (int)RespTypes.ObjectNull, msg = "未能找到第三方信息！" };

            var oauthUserRes = await OauthUserRep.Instance.GetById(tempOauthRes.data.userId);
            if (!oauthUserRes.IsSuccess())
                return new PortalTokenResp() { ret = (int)RespTypes.ObjectNull, msg = "未能找到第三方信息！" };

            return await OauthReg(oauthUserRes.data);
        }


        #endregion

        #region 辅助方法

        /// <summary>
        ///  根据第三方信息处理登录或注册
        ///     （如果未绑定过系统账号，根据配置处理是否默认注册绑定
        /// </summary>
        /// <param name="plat"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <param name="authType"></param>
        /// <returns></returns>
        private async Task<PortalTokenResp> OauthRegLogin(SocialPlatform plat, string code, string state, PortalAuthorizeType authType)
        {
            // 先获取第三方账号最新信息，更新至本地
            var oauthUserRes = await AddOrUpdateOauthUser(plat, code, state);
            if (!oauthUserRes.IsSuccess())
                return new PortalTokenResp().WithResp(oauthUserRes); // oauthUserRes.ConvertToResultInherit<PortalTokenResp>();

            // 【已绑定过的用户】   执行登录
            var oauthUser = oauthUserRes.data;
            if (oauthUser.owner_uid>0)
            {
                //  尝试直接登录
                var userRes = await UserInfoRep.Instance.GetById(oauthUser.owner_uid);
                return !userRes.IsSuccess()
                    ? new PortalTokenResp().WithResp(userRes)
                    : await LoginFinallyExecute(userRes.data, authType, false, plat);
            }

            //  管理员只能通过第三方绑定信息登录，如果没有，登录失败
            if (authType == PortalAuthorizeType.Admin)
                return new PortalTokenResp() { ret = (int)RespTypes.ParaError, msg = "非管理员账号，登录失败！" };

            // 【已绑定过的用户】  根据系统配置，检查是否默认绑定注册
            var regConfig = GetOauthRegConfig();
            if (regConfig.OauthRegisterType == OauthRegisterType.JustRegister)
            {
                return await OauthReg(oauthUser);
            }

            // 执行第三方临时授权，返回临时授权后通知前端，执行绑定相关操作
            oauthUser.status = regConfig.OauthRegisterType == OauthRegisterType.Bind
                ? UserStatus.WaitOauthBind
                : UserStatus.WaitOauthChooseBind;

            var ide = InitialOauthTempIdentity(oauthUser,plat);

            PortalEvents.TriggerOauthTempLoginEvent(ide.data, CoreAppContext.Identity, plat);
            return GeneratePortalToken(ide.data, plat);
        }


        /// <summary>
        ///     获取注册相关配置
        /// </summary>
        /// <para name="info">应用信息</para>
        /// <returns></returns>
        private static SocialRegisterConfig GetOauthRegConfig()
        {
            // todo 修改访问配置
            return new SocialRegisterConfig
            {
                OauthRegisterType = OauthRegisterType.JustRegister
            };
        }


        private async Task<PortalTokenResp> OauthReg(OauthUserMo oauthUser)
        {
            var user = GetRegisterUserInfo(oauthUser.nick_name, string.Empty, RegLoginType.ThirdName);
            user.avatar = oauthUser.head_img;

            var ptRes = await RegFinallyExecute(user, PortalAuthorizeType.User, false, oauthUser.social_plat);

            if (ptRes.IsSuccess())
                await OauthUserRep.Instance.BindUserIdByOauthId(oauthUser.id, ptRes.data.id.ToInt64());
            return ptRes;
        }

        /// <summary>
        ///   添加或更新第三方授权用户信息
        /// </summary>
        /// <param name="plat"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private static async Task<Resp<OauthUserMo>> AddOrUpdateOauthUser(SocialPlatform plat, string code,
            string state)
        {
            var userWxRes = await GetOauthUserByCode(plat, code, state);
            if (!userWxRes.IsSuccess())
                return new Resp<OauthUserMo>().WithResp(userWxRes);

            var userRes = await OauthUserRep.Instance.GetOauthUserByAppUserId(userWxRes.data.app_user_id, plat);
            if (userRes.IsSuccess())
            {
                //  如果存在，更新已有信息
                var user = userRes.data;

                user.SetInfoFromSocial(userWxRes.data);
                user.social_plat = plat;
                user.status = UserStatus.Normal;

                await OauthUserRep.Instance.UpdateUserWithToken(user);
                return new Resp<OauthUserMo>(user);
            }

            //  其他错误，直接返回
            if (!userRes.IsRespType(RespTypes.ObjectNull))
                return userRes;

            // 如果是新授权用户，添加新的信息
            var newUser = new OauthUserMo();
            newUser.SetInfoFromSocial(userWxRes.data);

            var idRes = await OauthUserRep.Instance.Add(newUser);
            if (!idRes.IsSuccess())
                return new Resp<OauthUserMo>().WithResp(idRes.ret, "添加授权用户信息失败！"); // idRes.ConvertToResult<OauthUserMo>();
            
            newUser.id = idRes.data;
            return new Resp<OauthUserMo>(newUser);
        }

        /// <summary>
        ///     获取授权用户并更新信息
        /// </summary>
        /// <param name="plat"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private static async Task<Resp<OauthUser>> GetOauthUserByCode(SocialPlatform plat, string code,
            string state)
        {
            var handler  = GetOauthAdapter(plat);
            var tokenRes = await handler.GetOauthTokenAsync(code, state);
            if (!tokenRes.IsSuccess())
                return new Resp<OauthUser>().WithResp(tokenRes); // tokenRes.ConvertToResult<OauthUserMo>();

            var userWxRes = await handler.GetOauthUserAsync(tokenRes.data.access_token, tokenRes.data.app_user_id);
            if (!userWxRes.IsSuccess())
                return new Resp<OauthUser>().WithResp(tokenRes); // tokenRes.ConvertToResult<OauthUserMo>();

            userWxRes.data.SetTokenInfo(tokenRes.data);
            return userWxRes;
        }

        private static IOauthAdapter GetOauthAdapter(SocialPlatform plat)
        {   
            return OauthAdapterHub.GetAdapter(plat.ToOauthPlat());
        }

        #endregion

    }
}