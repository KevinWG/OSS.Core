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
using OSS.Core.Domains.Members.Interfaces;
using OSS.Core.Domains.Members.Mos;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Services.Members.Exchange;
using OSS.Core.Infrastructure.Utils;

namespace OSS.Core.Services.Members
{
    public class PortalService
    {
        #region  用户手机号邮箱注册登录
        /// <summary>
        /// 注册用户信息
        /// </summary>
        /// <param name="value">注册的账号信息</param>
        /// <param name="passWord">密码</param>
        /// <param name="passCode"> 验证码（手机号注册时需要 </param>
        /// <param name="type">注册类型</param>
        /// <returns></returns>
        public async Task<UserTokenResp> RegisteUser(string value, string passWord, string passCode,
            RegLoginType type)
        {
            var checkRes = await CheckIfCanRegiste(type, value);
            if (!checkRes.IsSuccess()) return checkRes.ConvertToResult<UserTokenResp>();

            var userInfo = GetRegisteUserInfo(value, passWord, type);

            var idRes = await InsContainer<IUserInfoRep>.Instance.Insert(userInfo);
            if (!idRes.IsSuccess()) return idRes.ConvertToResult<UserTokenResp>();

            userInfo.Id = idRes.id;
            MemberEvents.TriggerUserRegiteEvent(userInfo, MemberShiper.AppAuthorize);

            return GenerateUserToken(userInfo.ConvertToMo(),MemberAuthorizeType.User);
        }

        private static UserInfoBigMo GetRegisteUserInfo(string value, string passWord, RegLoginType type)
        {
            var sysInfo = MemberShiper.AppAuthorize;

            var userInfo = new UserInfoBigMo
            {
                create_time = DateTime.Now.ToUtcSeconds(),
                app_source = sysInfo.AppSource,
                app_version = sysInfo.AppVersion
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
        ///  验证验证码是否正确
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="passcode"></param>
        /// <returns></returns>
        private async Task<ResultMo> CheckPasscode(string loginName, string passcode)
        {
            // todo check passcode
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
        public async Task<UserTokenResp> LoginUser(string name, string passWord,string passcode, RegLoginType type)
        {
            var rep = InsContainer<IUserInfoRep>.Instance;
            var userRes = await (type == RegLoginType.Mobile
                ? rep.Get<UserInfoBigMo>(u => u.mobile == name)
                : rep.Get<UserInfoBigMo>(u => u.email == name));

            if (!userRes.IsSuccess())
                return userRes.ConvertToResult<UserTokenResp>();

            if (Md5.EncryptHexString(passWord) != userRes.data.pass_word)
                return new UserTokenResp(ResultTypes.UnAuthorize, "账号密码不正确！");

            MemberEvents.TriggerUserLoginEvent(userRes.data, MemberShiper.AppAuthorize);

            var checkRes = CheckMemberStatus(userRes.data.status);

            return checkRes.IsSuccess()
                ? GenerateUserToken(userRes.data.ConvertToMo(),MemberAuthorizeType.User)
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

        private static UserTokenResp GenerateUserToken(UserInfoMo user, MemberAuthorizeType authType)
        {
            var tokenRes = AppendToken(user.Id, authType);

            return tokenRes.IsSuccess()
                ? new UserTokenResp() {token = tokenRes.data, user = user }
                : tokenRes.ConvertToResult<UserTokenResp>();
        }


        private static readonly string tokenSecret = ConfigUtil.GetSection("AppConfig:AppSecret")?.Value;
        public static ResultMo<(long id, int authType)> GetTokenDetail(string appSource, string tokenStr)
        {
            var tokenDetail = MemberShiper.GetTokenDetail(tokenSecret, tokenStr);

            var tokenSplit = tokenDetail.Split('|');
            return new ResultMo<ValueTuple<long, int>>((tokenSplit[0].ToInt64(), tokenSplit[1].ToInt32()));
        }

        public static ResultMo<string> AppendToken(long id, MemberAuthorizeType authType)
        {
            var tokenCon = string.Concat(id, "|", (int) authType, "|", DateTime.Now.ToUtcSeconds());
            return new ResultMo<string>(MemberShiper.GetToken(tokenSecret, tokenCon));
        }


        //private  static UserRegisteConfig GetRegisteconfig

    }
}
