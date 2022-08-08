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
using OSS.Common.Encrypt;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Domain;
using OSS.Core.Module.Notify;
using OSS.Tools.Cache;

namespace OSS.Core.Module.Portal
{
    // 修改绑定用户登录信息（短信，邮箱，第三方绑定登录信息） --- 不可通过密码修改
    //  1.  获取 bindtoken （发送用户 【短信/邮箱】 验证码获取） -- 主要是防止密码泄露后的账号篡改
    //  2.  通过 bindtoken 绑定新的信息
    
    // 修改登录账号：
    //    如果已经存在邮箱或手机号必须验证 BindToken
    // 修改密码：
    //     可以通过旧密码 和 BindToken 两种方式修改
    public class AuthBindingService : BaseAuthService
    {
        /// <summary>
        ///  获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        public Task<Resp<UserBasicMo>> GetCurrentUser()
        {
            return InsContainer<IUserService>.Instance.GetUserById(CoreContext.User.Identity.id.ToInt64());
        }
        
        /// <summary>
        ///  直接添加用户（管理员权限
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<Resp<long>> AddUser(AddUserReq req)
        {
            if (string.IsNullOrEmpty(req.email) && string.IsNullOrEmpty(req.mobile))
                return new Resp<long>().WithResp(RespCodes.ParaError, "手机号和邮箱不能同时为空").ToTaskResp();

            var user = req.MapToUserInfo();
            user.FormatBaseByContext();

            return RegAdd(user, 0);
        }

        #region 修改登录信息

        #region 动态码模式（手机号，邮箱）

        /// <summary>
        ///  发送当前登录账号动态码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<IResp> SendOldCode(PortalNameType type)
        {
            var userRes = await GetCurrentUser();
            if (!userRes.IsSuccess())
                return userRes;

            var user       = userRes.data;
            var notifyName = type == PortalNameType.Mobile ? user.mobile : user.email;

            if (string.IsNullOrEmpty(notifyName))
                return new Resp(RespCodes.OperateFailed, "账号信息为空，无法发送验证码");

            var code    = NumHelper.RandomNum();
            var sendRes = await Send(type, code, notifyName);

            var key = string.Concat(PortalConst.CacheKeys.Portal_Bind_Passcode_Old_ByType, type);
            await CacheHelper.SetAbsoluteAsync(key, code, TimeSpan.FromMinutes(2));

            return sendRes;
        }
        
        /// <summary>
        ///  发送新账号动态码
        /// </summary>
        /// <param name="portalName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<IResp> SendNewCode(PortalNameType type, string portalName)
        {
            var userRes = await GetCurrentUser();
            if (!userRes.IsSuccess())
                return userRes;

            var code    = NumHelper.RandomNum();
            var sendRes = await Send(type, code, portalName);

            var key = string.Concat(PortalConst.CacheKeys.Portal_Bind_Passcode_New_ByName, portalName);
            await CacheHelper.SetAbsoluteAsync(key, code, TimeSpan.FromMinutes(2));

            return sendRes;
        }

        /// <summary>
        ///  获取绑定信息的令牌
        /// </summary>
        /// <param name="oldCode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<GetBindTokenResp> GetBindToken(string oldCode, PortalNameType type)
        {
            var key      = string.Concat(PortalConst.CacheKeys.Portal_Bind_Passcode_Old_ByType, type);
            var checkRes = await CheckCodeCache(key, oldCode);

            if (!checkRes.IsSuccess())
                return new GetBindTokenResp().WithResp(checkRes);

            var bindToken = GenerateBindToken();
            return new GetBindTokenResp() { token = bindToken };
        }

        /// <summary>
        ///  绑定信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<IResp> BindByCode(BindByPassCodeReq req)
        {
            var key      = string.Concat(PortalConst.CacheKeys.Portal_Bind_Passcode_New_ByName, req.name);
            var checkRes = await CheckCodeCache(key, req.code);
            if (!checkRes.IsSuccess())
                return checkRes;

            var userInfoRes = await GetCurrentUser();
            if (!userInfoRes.IsSuccess())
                return userInfoRes;

            var user      = userInfoRes.data;
            var bindToken = req.bind_token;

            if (string.IsNullOrEmpty(bindToken) &&
                (!string.IsNullOrEmpty(user.mobile) || !string.IsNullOrEmpty(user.email)))
                return new Resp(RespCodes.OperateFailed, "绑定令牌不能为空!");

            if (!string.IsNullOrEmpty(bindToken) && !(checkRes = CheckBindToken(bindToken)).IsSuccess())
                return checkRes;

            // 判断新的账号是否存在
            var checkExistRes = await CheckNameIfCanReg(req);
            if (!checkExistRes.IsSuccess())
                return checkExistRes;

            return await m_AuthRep.UpdatePortalByType(user.id, req.type, req.name);
        }

        #region BindToken 加解密处理方法

        //  检查code
        private static async Task<IResp> CheckCodeCache(string chacheKey, string reqCode)
        {
            var code = await CacheHelper.GetAsync<string>(chacheKey);

            if (string.IsNullOrEmpty(code) || reqCode != code)
                return new Resp(RespCodes.OperateFailed, "验证码错误!");

            await CacheHelper.RemoveAsync(chacheKey);
            return new Resp();
        }

        private static string GenerateBindToken()
        {
            var data        = string.Concat(CoreContext.User.Identity.id, "|", DateTime.Now.ToUtcSeconds());
            var encryptData = AesRijndael.Encrypt(data, PortalTokenHelper.UserTokenSecret);
            return encryptData;
        }

        private static Resp CheckBindToken(string bindToken)
        {
            var dataDetail = AesRijndael.Decrypt(bindToken, PortalTokenHelper.UserTokenSecret);
            var dataStrs   = dataDetail.Split('|');
            if (dataStrs.Length != 2 || dataStrs[0] != CoreContext.User.Identity.id)
                return new Resp(RespCodes.OperateFailed, "令牌信息异常!");

            return new Resp();
        }

        private static async Task<IResp> Send(PortalNameType type, string code, string notifyName)
        {
            var tagets         = new List<string> {notifyName};

            var authSettingRes = await InsContainer<ISettingService>.Instance.GetAuthSetting();
            if (!authSettingRes.IsSuccess())
                return authSettingRes;

            var setting = authSettingRes.data;
            var templateId = type == PortalNameType.Mobile
                ? setting.SmsTemplateId.ToInt64()
                : setting.EmailTemplateId.ToInt64();

            var notifyMsg = new NotifyReq(tagets, templateId)
            {
                body_paras = new Dictionary<string, string> {{"code", code}}
            };

            return await InsContainer<INotifyClient>.Instance.NotifyService.Send(notifyMsg);
        }

        #endregion

        #endregion

        #endregion


        #region 登录等配置管理信息

        

        #endregion

    }

}