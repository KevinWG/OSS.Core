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
using OSS.Core.Domains.Core.Members.Interfaces;
using OSS.Core.Domains.Members.Mos;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Services.Members.Exchange;

namespace OSS.Core.Services.Members
{
    public class PortalService
    {
        /// <summary>
        /// 注册用户信息
        /// </summary>
        /// <param name="value">注册的账号信息</param>
        /// <param name="passWord">密码</param>
        /// <param name="passCode"> 验证码（手机号注册时需要 </param>
        /// <param name="type">注册类型</param>
        /// <returns></returns>
        public async Task<ResultMo<UserInfoMo>> RegisteUser(string value,string passWord,string passCode, RegLoginType type)
        {
            // todo 如果是手机号注册，检查验证码

            var checkRes =await CheckIfCanRegiste(type, value);
            if (!checkRes.IsSuccess()) return checkRes.ConvertToResultOnly<UserInfoMo>();
            
            var userInfo = GetRegisteUserInfo(value, passWord, type);

            var idRes =await InsContainer<IUserInfoRep>.Instance.Insert(userInfo);
            if (!idRes.IsSuccess()) return idRes.ConvertToResultOnly<UserInfoMo>();

            userInfo.Id = idRes.id;
            MemberEvents.TriggerUserRegiteEvent(userInfo, MemberShiper.AppAuthorize);

            return new ResultMo<UserInfoMo>(userInfo.ConvertToMo());
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

            if (type == RegLoginType.Email)
            {
                userInfo.email = value;
                userInfo.status = (int) MemberStatus.WaitConfirm;
            }
            else
                userInfo.mobile = value;

            if (type != RegLoginType.MobileCode)
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


        public async Task<ResultMo<UserInfoMo>> LoginUser(string name, string passWord, RegLoginType type)
        {
            var rep = InsContainer<IUserInfoRep>.Instance;
            var userRes=await (type == RegLoginType.Mobile
                ? rep.Get<UserInfoBigMo>(u => u.mobile == name)
                : rep.Get<UserInfoBigMo>(u => u.email == name));

         if (!userRes.IsSuccess())
                return userRes.ConvertToResultOnly<UserInfoMo>();

            if (Md5.EncryptHexString(passWord)!=userRes.data.pass_word)
            return new ResultMo<UserInfoMo>(ResultTypes.UnAuthorize,"账号密码不正确！");
          
            MemberEvents.TriggerUserLoginEvent(userRes.data,MemberShiper.AppAuthorize);

            var checkRes = CheckMemberStatus(userRes.data.status);

            return checkRes.IsSuccess()
                ? new ResultMo<UserInfoMo>(userRes.data.ConvertToMo())
                : checkRes.ConvertToResultOnly<UserInfoMo>();
        }

        /// <summary>
        /// 查看当前成员状态是否正常
        /// </summary>
        /// <returns></returns>
        public static ResultMo CheckMemberStatus(int state)
        {
            return state < (int)MemberStatus.WaitConfirm ? new ResultMo(ResultTypes.AuthFreezed, "此账号已经被锁定！") : new ResultMo();
        }


    }
}
