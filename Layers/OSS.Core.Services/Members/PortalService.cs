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

        /// <summary>
        ///  检查账号是否可以注册
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<ResultMo> CheckIfCanRegiste(RegLoginType type, string value)
        {
            return await InsContainer<IUserInfoRep>.Instance.CheckIfCanRegiste(type, value);
        }

        /// <summary>
        /// 注册用户信息
        /// </summary>
        /// <param name="name">注册的账号信息</param>
        /// <param name="passWord">密码</param>
        /// <param name="passcode"> 验证码（手机号注册时需要 </param>
        /// <param name="type">注册类型</param>
        /// <returns></returns>
        public async Task<UserTokenResp> RegisteUser(string name, string passWord, string passcode,
            RegLoginType type)
        {
            var codeRes = CheckPasscode(name, passcode);
            if (codeRes.IsSuccess())
                return codeRes.ConvertToResult<UserTokenResp>();

            var checkRes = await CheckIfCanRegiste(type, name);
            if (!checkRes.IsSuccess()) return checkRes.ConvertToResult<UserTokenResp>();

            var userInfo = GetRegisteUserInfo(name, passWord, type);

            var idRes = await InsContainer<IUserInfoRep>.Instance.Insert(userInfo);
            if (!idRes.IsSuccess()) return idRes.ConvertToResult<UserTokenResp>();

            userInfo.Id = idRes.id;
            MemberEvents.TriggerUserRegiteEvent(userInfo, MemberShiper.AppAuthorize);

            return GenerateUserToken(userInfo, MemberAuthorizeType.User);
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

        /// <summary>
        ///  用户登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="passWord"></param>
        /// <param name="passcode">动态验证码</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<UserTokenResp> LoginUser(string name, string passWord, string passcode, RegLoginType type)
        {
            var isCodeCheck = !string.IsNullOrEmpty(passcode);
            if (isCodeCheck)
            {
                var codeRes = CheckPasscode(name, passcode);
                if (codeRes.IsSuccess())
                    return codeRes.ConvertToResult<UserTokenResp>();
            }

            var userRes = await InsContainer<IUserInfoRep>.Instance.GetUserByLoginType(name, type);
            if (!userRes.IsSuccess())
                return userRes.ConvertToResult<UserTokenResp>();

            if (!isCodeCheck)
            {
                if (string.IsNullOrEmpty(userRes.data.pass_word))
                    return new UserTokenResp(ResultTypes.ObjectStateError, "当前账号需要动态码登录！");
                if (Md5.EncryptHexString(passWord) != userRes.data.pass_word)
                    return new UserTokenResp(ResultTypes.UnAuthorize, "账号密码不正确！");
            }

            MemberEvents.TriggerUserLoginEvent(userRes.data, MemberShiper.AppAuthorize);
            var checkRes = CheckMemberStatus(userRes.data.status);

            return checkRes.IsSuccess()
                ? GenerateUserToken(userRes.data, MemberAuthorizeType.User)
                : checkRes.ConvertToResult<UserTokenResp>();
        }

        /// <summary>
        /// 查看当前成员状态是否正常
        /// </summary>
        /// <returns></returns>
        public static ResultMo CheckMemberStatus(int state)
        {
            return state < (int) MemberStatus.WaitOauthChooseBind
                ? new ResultMo(ResultTypes.AuthFreezed, "此账号已经被锁定！")
                : new ResultMo();
        }

        #region    第三方授权模块

        ///// <summary>
        ///// 注册第三方信息到系统中
        ///// </summary>
        ///// <param name="plat"></param>
        ///// <param name="code"></param>
        ///// <param name="state"></param>
        ///// <returns></returns>
        //public async Task<ResultMo<OauthUserMo>> RegisteThirdUser(ThirdPaltforms plat, string code, string state)
        //{
        //    var handler = GetHandlerByPlatform(plat);
        //    var tokenRes = await handler.GetOauthTokenAsync(code, state);
        //    if (!tokenRes.IsSuccess())
        //        return tokenRes.ConvertToResultOnly<OauthUserMo>();

        //    var userRes = await InsContainer<IOauthUserRep>.Instance.GetOauthUserByAppUId(tokenRes.data.app_user_id,
        //        ThirdPaltforms.Wechat);

        //    var userWxRes = await handler.GetOauthUserAsync(tokenRes.data.access_token, tokenRes.data.app_user_id);
        //    if (userWxRes.IsSuccess())
        //    {

        //    }

        //    if (userRes.IsSuccess())
        //    {
        //        //  执行修改
        //    }
        //    else
        //    {
        //        // 执行新增
        //    }

        //    //  

        //}


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
            var oauthUserRes = await SnsCommon.GetOauthUserByCode(plat, code, state);
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

        private static readonly string tokenSecret = ConfigUtil.GetSection("AppConfig:AppSecret")?.Value;

        public static ResultMo<(long id, int authType)> GetTokenDetail(string appSource, string tokenStr)
        {
            var tokenDetail = MemberShiper.GetTokenDetail(tokenSecret, tokenStr);

            var tokenSplit = tokenDetail.Split('|');
            return new ResultMo<ValueTuple<long, int>>((tokenSplit[0].ToInt64(), tokenSplit[1].ToInt32()));
        }
        private static UserTokenResp GenerateUserToken(UserInfoBigMo user, MemberAuthorizeType authType)
        {
            var tokenCon = string.Concat(user.Id, "|", (int) authType, "|", DateTime.Now.ToUtcSeconds());
            var token = MemberShiper.GetToken(tokenSecret, tokenCon);
            return new UserTokenResp() {token = token, user = user.ConvertToMo()};
        }
        
        #endregion



    }
}
