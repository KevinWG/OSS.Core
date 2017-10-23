#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 登录注册入口 service （前后台用户信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-17
*       
*****************************************************************************/

#endregion

using System;
using System.Threading.Tasks;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.ComUtils;
using OSS.Common.Encrypt;
using OSS.Common.Extention;
using OSS.Common.Plugs.CachePlug;
using OSS.Core.Domains.Members.Interfaces;
using OSS.Core.Domains.Members.Mos;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Services.Members.Exchange;
using OSS.Core.Infrastructure.Utils;
using OSS.Core.Services.Sns.Exchange;

namespace OSS.Core.Services.Members
{
    public class PortalService
    {
        #region  用户手机号邮箱注册登录

        #region  登录

        /// <summary>
        ///  检查账号是否可以注册
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<ResultMo> CheckIfCanReg(RegLoginType type, string value)
        {
            return await InsContainer<IUserInfoRep>.Instance.CheckIfCanRegiste(type, value);
        }

        /// <summary>
        /// 注册用户信息
        /// </summary>
        /// <param name="name">注册的账号信息</param>
        /// <param name="passWord">密码</param>
        /// <param name="type">注册类型</param>
        /// <returns></returns>
        public async Task<UserTokenResp> UserReg(string name, string passWord,RegLoginType type)
        {
            var checkRes = await CheckIfCanReg(type, name);
            if (!checkRes.IsSuccess()) return checkRes.ConvertToResult<UserTokenResp>();

            return await RegExcute(name, passWord, type);
        }

        private static async Task<UserTokenResp> RegExcute(string name, string passWord, RegLoginType type)
        {
            var userInfo = GetRegisteUserInfo(name, passWord, type);

            var idRes = await InsContainer<IUserInfoRep>.Instance.Insert(userInfo);
            if (!idRes.IsSuccess()) return idRes.ConvertToResult<UserTokenResp>();

            userInfo.Id = idRes.id;
            MemberEvents.TriggerUserRegiteEvent(userInfo, MemberShiper.AppAuthorize);

            return BindOauthAndGenerateUserToken(userInfo, MemberAuthorizeType.User);
        }

 

        /// <summary>
        ///  用户登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="passWord"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<UserTokenResp> UserLogin(string name, string passWord,  RegLoginType type)
        {
            var userRes = await InsContainer<IUserInfoRep>.Instance.GetUserByLoginType(name, type);
            if (!userRes.IsSuccess())
                return userRes.ConvertToResult<UserTokenResp>();

            var user = userRes.data;
            return Md5.EncryptHexString(passWord) != user.pass_word 
                ? new UserTokenResp(ResultTypes.UnAuthorize, "账号密码不正确！")
                : LoginExcute(user);
        }

        private static UserTokenResp LoginExcute(UserInfoBigMo user)
        {
            MemberEvents.TriggerUserLoginEvent(user, MemberShiper.AppAuthorize);
            var checkRes = CheckMemberStatus(user.status);

            return checkRes.IsSuccess()
                ? BindOauthAndGenerateUserToken(user, MemberAuthorizeType.User)
                : checkRes.ConvertToResult<UserTokenResp>();
        }

        /// <summary>
        /// 查看当前成员状态是否正常
        /// </summary>
        /// <returns></returns>
        public static ResultMo CheckMemberStatus(int state)
        {
            return state < (int)MemberStatus.WaitOauthChooseBind
                ? new ResultMo(ResultTypes.AuthFreezed, "此账号已经被锁定！")
                : new ResultMo();
        }

        #endregion


        #region  动态验证码登录

        /// <summary>
        ///  发送验证码
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<ResultMo> SendVertifyCode(string name, RegLoginType type)
        {
            // todo 发送验证码
            return new ResultMo();
        }
        
        /// <summary>
        ///  动态码登录
        ///     如果用户不存在，则自动注册
        /// </summary>
        /// <param name="name"></param>
        /// <param name="passcode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<UserTokenResp> CodeLogin(string name, string passcode, RegLoginType type)
        {
            var codeRes = CheckPasscode(name, passcode);
            if (codeRes.IsSuccess())
                return codeRes.ConvertToResult<UserTokenResp>();

            var userRes = await InsContainer<IUserInfoRep>.Instance.GetUserByLoginType(name, type);
            if (!userRes.IsSuccess() && !userRes.IsResultType(ResultTypes.ObjectNull))
                return userRes.ConvertToResult<UserTokenResp>();

            // 执行注册
            if (userRes.IsResultType(ResultTypes.ObjectNull))
                return await RegExcute(name, null, type);
            // 执行登录
            return LoginExcute(userRes.data);
        }


        /// <summary>
        ///  验证验证码是否正确
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="passcode"></param>
        /// <returns></returns>
        private static ResultMo CheckPasscode(string loginName, string passcode)
        {
            var key = string.Concat(GlobalKeysUtil.RegLoginVertifyCodePre, loginName);
            var code = CacheUtil.Get<string>(key);

            if (string.IsNullOrEmpty(code) || passcode != code)
                return new ResultIdMo(ResultTypes.ObjectStateError, "验证码错误");

            CacheUtil.Remove(key);
            return new ResultIdMo();
        }

        #endregion 


        #endregion

        #region 社交平台授权登录注册

        /// <summary>
        /// 注册第三方信息到系统中
        /// </summary>
        /// <param name="plat"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<UserTokenResp> SocialAuth(SocialPaltforms plat, string code, string state)
        {
            var oauthUserRes = await SetOauthUser(plat, code, state);
            if (!oauthUserRes.IsSuccess())
                return oauthUserRes.ConvertToResult<UserTokenResp>();

            UserInfoBigMo user;
            var type = MemberAuthorizeType.User;

            var oauthUser = oauthUserRes.data;
            if (oauthUser.user_id > 0) // 已经存在绑定，直接登录成功
            {
                var userRes =
                    await InsContainer<IUserInfoRep>.Instance.Get<UserInfoBigMo>(u => u.Id == oauthUser.user_id);
                if (!userRes.IsSuccess())
                    return userRes.ConvertToResult<UserTokenResp>();

                user = userRes.data;
                MemberEvents.TriggerUserLoginEvent(user, MemberShiper.AppAuthorize);
            }
            else
            {
                var regConfig = GetRegConfig(MemberShiper.AppAuthorize);
                user = oauthUser.ConvertToBigMo();
                if (regConfig.OauthRegisteType == OauthRegisteType.JustRegiste)
                {
                    // 授权后直接注册用户
                    var idRes = await InsContainer<IUserInfoRep>.Instance.Insert(user);
                    if (!idRes.IsSuccess())
                        return idRes.ConvertToResult<UserTokenResp>();

                    user.Id = idRes.id;

#pragma warning disable 4014
                    InsContainer<IOauthUserRep>.Instance.UpdateUserIdByOauthId(oauthUser.Id, user.Id);
#pragma warning restore 4014
                    MemberEvents.TriggerUserLoginEvent(user, MemberShiper.AppAuthorize);
                }
                else
                {
                    // 授权后通知前端，执行绑定相关操作
                    user.Id = oauthUser.Id;
                    user.status = regConfig.OauthRegisteType == OauthRegisteType.Bind
                        ? (int) MemberStatus.WaitOauthBind
                        : (int) MemberStatus.WaitOauthChooseBind;
                    type = MemberAuthorizeType.OauthUserTemp;
                }
            }
            return GenerateUserToken(user, type);
        }


        private static async Task<ResultMo<OauthUserMo>> SetOauthUser(SocialPaltforms plat, string code, string state)
        {
            var userWxRes = await SnsCommon.GetOauthUserByCode(plat, code, state);
            var userRes = await InsContainer<IOauthUserRep>.Instance.GetOauthUserByAppUId(
                MemberShiper.AppAuthorize.TenantId.ToInt64(),
                userWxRes.data.app_user_id, plat);

            if (userRes.IsSuccess())
            {
                var user = userRes.data;
                user.SetFromSocial(userWxRes.data);

                await InsContainer<IOauthUserRep>.Instance.UpdateUserWithToken(user);
                return new ResultMo<OauthUserMo>(user);
            }

            if (!userRes.IsResultType(ResultTypes.ObjectNull))
                return userRes;

            var newUser = userWxRes.data;
            var idRes = await InsContainer<IOauthUserRep>.Instance.Insert(newUser);

            if (!idRes.IsSuccess())
                return idRes.ConvertToResultOnly<OauthUserMo>();

            newUser.Id = idRes.id;
            return new ResultMo<OauthUserMo>(newUser);
        }

        #endregion



        #region 辅助方法
        /// <summary>
        ///  获取注册相关配置
        /// </summary>
        /// <para name="info">应用信息</para>
        /// <returns></returns>
        private static UserRegisteConfig GetRegConfig(AppAuthorizeInfo info)
        {
            return new UserRegisteConfig()
            {
                OauthRegisteType = OauthRegisteType.JustRegiste
            };
        }
        
        private static UserTokenResp BindOauthAndGenerateUserToken(UserInfoBigMo user, MemberAuthorizeType authType)
        {
            if (MemberShiper.IsAuthenticated
                &&MemberShiper.Identity.AuthenticationType==(int)MemberAuthorizeType.OauthUserTemp)
            {
               var OauthUserId = MemberShiper.Identity.Id;
                if (OauthUserId > 0)
                {
                    InsContainer<IOauthUserRep>.Instance.UpdateUserIdByOauthId(OauthUserId, user.Id);
                }
            }
            return GenerateUserToken(user, authType);
        }

        private static readonly string tokenSecret = ConfigUtil.GetSection("AppConfig:AppSecret")?.Value;
        
        private static UserTokenResp GenerateUserToken(UserInfoBigMo user, MemberAuthorizeType authType)
        {
            var tokenStr = string.Concat(user.Id, "|", (int) authType, "|", DateTime.Now.ToUtcSeconds());
            var token = MemberShiper.GetToken(tokenSecret, tokenStr);
            return new UserTokenResp() {token = token, user = user.ConvertToMo()};
        }
        public static ResultMo<(long id, int authType)> GetTokenDetail(string appSource, string tokenStr)
        {
            var tokenDetail = MemberShiper.GetTokenDetail(tokenSecret, tokenStr);

            var tokenSplit = tokenDetail.Split('|');
            return new ResultMo<ValueTuple<long, int>>((tokenSplit[0].ToInt64(), tokenSplit[1].ToInt32()));
        }

        private static UserInfoBigMo GetRegisteUserInfo(string value, string passWord, RegLoginType type)
        {
            var sysInfo = MemberShiper.AppAuthorize;

            var userInfo = new UserInfoBigMo
            {
                create_time = DateTime.Now.ToUtcSeconds(),
                app_source = sysInfo.AppSource,
                app_version = sysInfo.AppVersion,
                tenant_id = sysInfo.TenantId.ToInt64()
            };

            if (type == RegLoginType.Mobile)
                userInfo.mobile = value;
            else
                userInfo.email = value;

            if (!string.IsNullOrEmpty(passWord))
                userInfo.pass_word = Md5.EncryptHexString(passWord);

            return userInfo;
        }
        #endregion



    }
}
