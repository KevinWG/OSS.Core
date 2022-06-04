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

using OSS.Common;
using OSS.Common.Encrypt;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Extension;
using OSS.Core.Portal.Domain;
using OSS.Core.Portal.Service;
using OSS.Core.Portal.Service.Common.Helpers;
using OSS.Core.Portal.Shared.IService;

namespace OSS.Core.Services.Basic.Portal
{
    public partial class PortalService : BasePortalService, ISharedPortalService
    {
        #region 获取登录认证信息

        /// <summary>
        ///  获取授权账号信息
        /// </summary>
        /// <returns></returns>
        public Task<Resp<UserIdentity>> GetIdentity()
        {
            var cacheKey = string.Concat(PortalConst.CacheKeys.Portal_UserIdentity_ByToken, CoreContext.App.Identity.token);
            Func<Task<Resp<UserIdentity>>> getFunc = () =>
            {
                var infoRes = PortalTokenHelper.FormatPortalToken();
                if (!infoRes.IsSuccess())
                    return Task.FromResult(new Resp<UserIdentity>().WithResp(infoRes));

                return GetAuthIdentityById(infoRes.data.userId, infoRes.data.authType);
            };
            return getFunc.WithAbsoluteCache(cacheKey, TimeSpan.FromMinutes(5));
        }

        private static async Task<Resp<UserIdentity>> GetAuthIdentityById(long userId, PortalAuthorizeType authType)
        {
            Resp<UserIdentity> identityRes;
            switch (authType)
            {
                case PortalAuthorizeType.Admin:
                case PortalAuthorizeType.SuperAdmin:
                    identityRes = await PortalIdentityHelper.GetAdminIdentity(userId);
                    break;

                case PortalAuthorizeType.User:
                case PortalAuthorizeType.UserWithEmpty:
                    identityRes = await PortalIdentityHelper.GetUserIdentity(userId);
                    break;
                default:
                    identityRes = new Resp<UserIdentity>().WithResp(RespTypes.UserUnLogin, "用户未登录!");
                    break;
            }

            if (!identityRes.IsSuccess() && !identityRes.IsRespType(RespTypes.UserBlocked) && !identityRes.IsRespType(RespTypes.UserUnActive))
            {
                // 正常请求获取登录信息时异常，统一返回未登录错误码
                identityRes.WithResp(RespTypes.UserUnLogin, identityRes.msg);
            }
            return identityRes;
        }

        #endregion

        #region 验证是否可以注册

        /// <summary>
        ///     检查账号是否可以注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Resp> CheckIfCanReg(PortalNameReq req)
        {
            var userRes = await  InsContainer<IUserInfoRep>.Instance.GetUserByLoginType(req.name, req.type);
            if (userRes.IsRespType(RespTypes.OperateObjectNull))
                return new Resp();

            return userRes.IsSuccess()
                ? new Resp(RespTypes.OperateObjectExist, "账号已存在，无法注册！")
                : new Resp().WithResp(userRes);
        }

        #endregion

        #region 账号密码登录注册实现

        /// <summary>
        ///   直接通过账号密码注册用户信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PortalTokenResp> PwdReg(PortalPasswordReq req)
        {
            var checkRes = await CheckIfCanReg(req);
            if (!checkRes.IsSuccess()) return new PortalTokenResp().WithResp(checkRes); // checkRes.ConvertToResultInherit<PortalTokenResp>();

            var userInfo = GetRegisterUserInfo(req.name, req.password, req.type.ToPortalType());

            userInfo.status = UserStatus.WaitActive;//  默认待激活状态
            return await RegFinallyExecute(userInfo, req.is_social_bind);
        }

        /// <summary> 
        ///     用户密码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PortalTokenResp> PwdLogin(PortalPasswordReq req)
        {
            return PwdLogin(req, false);
        }

        /// <summary>
        ///     管理员用户密码登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PortalTokenResp> PwdAdminLogin(PortalPasswordReq req)
        {
            return PwdLogin(req, true);
        }

        private async Task<PortalTokenResp> PwdLogin(PortalPasswordReq req, bool isAdmin)
        {
            var userRes = await InsContainer<IUserInfoRep>.Instance.GetUserByLoginType(req.name, req.type);
            if (userRes.IsSuccess())
            {
                var user = userRes.data;
                if (Md5.EncryptHexString(req.password) == user.pass_word)
                {
                    return await LoginFinallyExecute(user, isAdmin, req.is_social_bind);
                }
            }
            return new PortalTokenResp(RespTypes.ParaError, "账号密码不正确！");
        }


        #endregion
    }
}