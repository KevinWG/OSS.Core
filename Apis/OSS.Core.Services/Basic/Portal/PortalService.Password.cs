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
using OSS.Common.BasicMos.Resp;
using OSS.Common.Encrypt;
using OSS.Core.Context.Mos;
using OSS.Core.Infrastructure.BasicMos.Enums;
using OSS.Core.Infrastructure.BasicMos.Enums;
using OSS.Core.RepDapper.Basic.Portal;
using OSS.Core.Services.Basic.Portal.Mos;
using UserStatus = OSS.Core.RepDapper.Basic.Portal.Mos.UserStatus;

namespace OSS.Core.Services.Basic.Portal
{
    public partial class PortalService
    {
        /// <summary>
        ///   直接通过账号密码注册用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="type"></param>
        /// <param name="isFromThirdBind"> 是否来自第三方授权后绑定页的登录注册请求 </param>
        /// <returns></returns>
        public async Task<PortalTokenResp> PwdReg(string name, string password, RegLoginType type, bool isFromThirdBind=false)
        {
            var checkRes = await CheckIfCanReg(type, name);
            if (!checkRes.IsSuccess()) return new PortalTokenResp().WithResp(checkRes); // checkRes.ConvertToResultInherit<PortalTokenResp>();

            var userInfo = GetRegisterUserInfo(name, password, type);

            userInfo.status = UserStatus.WaitActive;//  默认待激活状态
            return await RegFinallyExecute(userInfo, PortalAuthorizeType.User, isFromThirdBind);
        }

        /// <summary>
        ///     用户密码登录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="isFromThirdBind"> 是否来自第三方授权后绑定页的登录注册请求 </param>
        /// <returns></returns>
        public Task<PortalTokenResp> PwdLogin(string name, string password, RegLoginType type, bool isFromThirdBind=false)
        {
           return PwdLogin(name, password, type,  PortalAuthorizeType.User,isFromThirdBind);
        }

        /// <summary>
        ///     管理员用户密码登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="type"></param>
        /// <param name="isFromThirdBind"> 是否来自第三方授权后绑定页的登录注册请求 </param>
        /// <returns></returns>
        public Task<PortalTokenResp> PwdAdminLogin(string name, string password, RegLoginType type, bool isFromThirdBind=false)
        {
            return PwdLogin( name,  password,  type,   PortalAuthorizeType.Admin,isFromThirdBind);
        }

        private async Task<PortalTokenResp> PwdLogin(string name, string password, RegLoginType type, PortalAuthorizeType authType, bool isFromThirdBind)
        {
            var userRes = await UserInfoRep.Instance.GetUserByLoginType(name, type);
            if (userRes.IsSuccess())
            {
                var user = userRes.data;
                if (Md5.EncryptHexString(password) == user.pass_word)
                {
                    return await LoginFinallyExecute(user, authType, isFromThirdBind);
                }
            }
            return new PortalTokenResp(RespTypes.ParaError, "账号密码不正确！");
        }

    }
}