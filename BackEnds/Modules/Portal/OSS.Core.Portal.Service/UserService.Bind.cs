#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 成员信息领域Service （前后台用户信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-6-4
*       
*****************************************************************************/

#endregion

using OSS.Common;
using OSS.Common.Resp;
using OSS.Common.Encrypt;
using OSS.Common.Extension;
using OSS.Core.Context;
using OSS.Tools.Cache;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Core.Common.Const;
using OSS.Core.Reps.Basic.Portal;
using OSS.Core.Reps.Basic.Portal.Mos;
using OSS.Core.Services.Basic.Portal.Helpers;
using OSS.Core.Services.Basic.Portal.IProxies;
using OSS.Core.Services.Basic.Portal.Reqs;
using OSS.Core.Services.Plugs.Notify;
using OSS.Core.Services.Plugs.Notify.Mos;

namespace OSS.Core.Services.Basic.Portal
{
    // 修改绑定用户登录信息（短信，邮箱，第三方绑定登录信息） --- 不可通过密码修改
    //  1.  获取 bindtoken （发送用户 【短信/邮箱】 验证码获取） -- 主要是防止密码泄露后的账号篡改
    //  2.  通过 bindtoken 绑定新的信息

    // 修改登录账号：
    //    如果已经存在邮箱或手机号必须验证 BindToken
    // 修改密码：
    //     可以通过旧密码 和 BindToken 两种方式修改
    public partial class UserService
    {
        /// <summary>
        ///  发送动态码
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<Resp> SendOldCode(PortalCodeType type)
        {
            var userRes = await GetMyInfo();
            if (!userRes.IsSuccess())
                return userRes;

            var user = userRes.data;
            var notifyName = type == PortalCodeType.Mobile ? user.mobile : user.email;

            if (string.IsNullOrEmpty(notifyName))
                return new Resp(RespTypes.OperateFailed, "账号信息为空，无法发送验证码");

            var code = OSS.Common.Helpers.NumHelper.RandomNum();
            var sendRes = await Send(type, code, notifyName);

            var key = string.Concat(CacheKeys.Portal_Bind_Passcode_Old_ByType, type);
            await CacheHelper.SetAbsoluteAsync(key, code, TimeSpan.FromMinutes(2));

            return sendRes;
        }

        /// <summary>
        ///  获取绑定信息的令牌
        /// </summary>
        /// <param name="oldCode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<GetBindTokenResp> GetBindToken(string oldCode, PortalCodeType type)
        {
            var key = string.Concat(CacheKeys.Portal_Bind_Passcode_Old_ByType, type);
            var checkRes = await CheckCodeCache(key, oldCode);

            if (!checkRes.IsSuccess())
                return new GetBindTokenResp().WithResp(checkRes);

            string bindToken = GenerateBindToken();
            return new GetBindTokenResp() { token = bindToken };
        }

        /// <summary>
        ///  发送新账号动态码
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<Resp> SendNewCode(PortalCodeType type, string portalName)
        {
            var userRes = await GetMyInfo();
            if (!userRes.IsSuccess())
                return userRes;

            var code = OSS.Common.Helpers.NumHelper.RandomNum();
            var sendRes = await Send(type, code, portalName);

            var key = string.Concat(CacheKeys.Portal_Bind_Passcode_New_ByName, portalName);
            await CacheHelper.SetAbsoluteAsync(key, code, TimeSpan.FromMinutes(2));

            return sendRes;
        }

        /// <summary>
        ///  绑定信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Resp> Bind(BindUserPortalReq req)
        {
            var key = string.Concat(CacheKeys.Portal_Bind_Passcode_New_ByName, req.name);
            var checkRes = await CheckCodeCache(key, req.code);
            if (!checkRes.IsSuccess())
                return checkRes;

            var userInfoRes = await GetMyInfo();
            if (!userInfoRes.IsSuccess())
                return userInfoRes;

            var user = userInfoRes.data;
            var bindToken = req.bind_token;

            if (string.IsNullOrEmpty(bindToken) &&
                (!string.IsNullOrEmpty(user.mobile) || !string.IsNullOrEmpty(user.email)))
                return new Resp(RespTypes.OperateFailed, "绑定令牌不能为空!");

            if (!string.IsNullOrEmpty(bindToken) && !(checkRes = CheckBindToken(bindToken)).IsSuccess())
                return checkRes;

            // 判断新的账号是否存在
            var checkExistRes = await InsContainer<IPortalServiceProxy>.Instance.CheckIfCanReg(req);
            if (!checkExistRes.IsSuccess())
                return checkExistRes;

            return await UserInfoRep.Instance.UpdatePortalByType(user.id, req.type, req.name);
        }

        #region BindToken 处理方法

        //  检查code
        private static async Task<Resp> CheckCodeCache(string chacheKey, string reqCode)
        {
            var code = await CacheHelper.GetAsync<string>(chacheKey);

            if (string.IsNullOrEmpty(code) || reqCode != code)
                return new Resp(RespTypes.OperateFailed, "验证码错误!");

            await CacheHelper.RemoveAsync(chacheKey);
            return new Resp();
        }

        private static string GenerateBindToken()
        {
            var data = string.Concat(CoreUserContext.Identity.id, "|", DateTime.Now.ToUtcSeconds());
            var encryptData = AesRijndael.Encrypt(data, PortalTokenHelper.PortalTokenSecret);
            return encryptData;
        }

        private static Resp CheckBindToken(string bindToken)
        {
            var dataDetail = AesRijndael.Decrypt(bindToken, PortalTokenHelper.PortalTokenSecret);
            var dataStrs = dataDetail.Split('|');
            if (dataStrs.Length != 2 || dataStrs[0] != CoreUserContext.Identity.id)
            {
                return new Resp(RespTypes.OperateFailed, "令牌信息异常!");
            }

            return new Resp();
        }

        private static async Task<Resp> Send(PortalCodeType type, string code, string notifyName)
        {
            var notifyMsg = new NotifyReq
            {
                targets = new List<string>() { notifyName },
                body_paras = new Dictionary<string, string> { { "code", code } },

                t_code = type == PortalCodeType.Mobile
                    ? DirConfigKeys.plugs_notify_sms_portal_tcode
                    : DirConfigKeys.plugs_notify_email_portal_tcode
            };

            return await InsContainer<INotifyService>.Instance.Send(notifyMsg);
        }
        #endregion

    }

}