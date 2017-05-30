#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 成员信息领域Service （前后台用户信息
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
using OSS.Common.ComUtils;
using OSS.Common.Encrypt;
using OSS.Core.Domains.Members.Interfaces;
using OSS.Core.Domains.Members.Mos;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Services.Members.Exchange;

namespace OSS.Core.Services.Members
{
    public class MemberService
    {
        /// <summary>
        /// 获取前台用户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public async Task<ResultMo<UserInfoMo>> GetUserInfo(long userId)
        {
            return await MemberCommon.GetUserInfo(userId);
        }

        /// <summary>
        ///  获取后台管理员信息
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public async Task<ResultMo<AdminInfoMo>> GetAdminInfo(long adminId)
        {
            return await InsContainer<IAdminInfoRep>.Instance.Get<AdminInfoMo>();
        }

        /// <summary>
        /// 注册用户信息
        /// </summary>
        /// <param name="value">注册的账号信息</param>
        /// <param name="password">密码</param>
        /// <param name="passCode"> 验证码（手机号注册时需要 </param>
        /// <param name="type">注册类型</param>
        /// <param name="auInfo">注册的系统信息</param>
        /// <returns></returns>
        public async Task<ResultMo<UserInfoMo>> RegisteUser(string value,string password,string passCode, RegLoginType type, SysAuthorizeInfo auInfo)
        {
            var checkRes =await CheckIfCanRegiste(type, value);
            if (!checkRes.IsSuccess()) return checkRes.ConvertToResultOnly<UserInfoMo>();

            // todo 检查验证码

            var userInfo=new UserInfoBigMo();

            if (type == RegLoginType.Email)
                userInfo.email = value;
            else userInfo.mobile = value;

            if (type != RegLoginType.MobileCode)
                userInfo.pass_word = Md5.HalfEncryptHexString(passCode);

            var idRes =await InsContainer<IUserInfoRep>.Instance.Insert(userInfo);
            if (!idRes.IsSuccess()) return idRes.ConvertToResultOnly<UserInfoMo>();

            userInfo.Id = idRes.id;
            // todo 触发新用户注册事件
            return new ResultMo<UserInfoMo>(userInfo);
        }

        //public async Task<ResultMo<UserInfoMo>> LoginUser(string name, string pass_code, RegLoginType type,
        //    SysAuthorizeInfo appAuthorize)
        //{
        //    var userRes=InsContainer
        //}

        /// <summary>
        ///  检查账号是否可以注册
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<ResultMo> CheckIfCanRegiste(RegLoginType type, string value)
        {
            return await InsContainer<IUserInfoRep>.Instance.CheckIfCanRegiste(type,value);
        }
    }
}
