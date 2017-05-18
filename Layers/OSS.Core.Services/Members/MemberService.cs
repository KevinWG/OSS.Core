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

using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Common.Encrypt;
using OSS.Core.DomainMos;
using OSS.Core.DomainMos.Members.Interfaces;
using OSS.Core.DomainMos.Members.Mos;
using OSS.Core.Infrastructure.Enums;

namespace OSS.Core.Services.Members
{
    public class MemberService
    {
        /// <summary>
        /// 获取前台用户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public  ResultMo<UserInfoMo> GetUserInfo(long userId)
        {
            return MemberManager.GetUserInfo(userId);
        }

        /// <summary>
        ///  获取后台管理员信息
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public ResultMo<AdminInfoMo> GetAdminInfo(long adminId)
        {
            return Rep<IAdminInfoRep>.Instance.Get<AdminInfoMo>();
        }

        /// <summary>
        /// 注册用户信息
        /// </summary>
        /// <param name="value">注册的账号信息</param>
        /// <param name="password">密码</param>
        /// <param name="type">注册类型</param>
        /// <param name="auInfo">注册的系统信息</param>
        /// <returns></returns>
        public ResultMo<UserInfoMo> RegisteUser(string value,string password, RegLoginType type, SysAuthorizeInfo auInfo)
        {
            var checkRes = CheckIfCanRegiste(type, value);
            if (!checkRes.IsSuccess) return checkRes.ConvertToResultOnly<UserInfoMo>();

            var userInfo=new UserInfoBigMo();

            if (type == RegLoginType.Email)
                userInfo.email = value;
            else userInfo.mobile = value;

            if (type != RegLoginType.MobileCode)
                userInfo.pass_word = Md5.HalfEncryptHexString(password);

            var idRes = Rep<IUserInfoRep>.Instance.Insert(userInfo);
            if (!idRes.IsSuccess) return idRes.ConvertToResultOnly<UserInfoMo>();

            userInfo.Id = idRes.Id;
            // todo 触发新用户注册事件
            return new ResultMo<UserInfoMo>(userInfo);
        }

        /// <summary>
        ///  检查账号是否可以注册
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ResultMo CheckIfCanRegiste(RegLoginType type, string value)
        {
            return Rep<IUserInfoRep>.Instance.CheckIfCanRegiste(type,value);
        }
    }
}
