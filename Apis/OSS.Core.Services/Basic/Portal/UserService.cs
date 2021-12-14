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

using System.Threading.Tasks;
using OSS.Common;
using OSS.Common.BasicMos;
using OSS.Common.Resp;
using OSS.Core.RepDapper;
using OSS.Core.RepDapper.Basic.Portal;
using OSS.Core.RepDapper.Basic.Portal.Mos;
using OSS.Core.Services.Basic.Portal.IProxies;

namespace OSS.Core.Services.Basic.Portal
{
    public partial class UserService :  IUserServiceProxy
    {
        /// <summary>
        ///  添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Resp<long>> AddUser(UserInfoBigMo user)
        {
            if (!string.IsNullOrEmpty(user.email))
            {
                var checkEmailRes =await InsContainer<IPortalServiceProxy>.Instance.CheckIfCanReg(RegLoginType.Email, user.email);
                if (!checkEmailRes.IsSuccess())
                    return  new Resp<long>().WithResp(checkEmailRes);
            }
            if (!string.IsNullOrEmpty(user.mobile))
            {
                var checkMobileRes = await InsContainer<IPortalServiceProxy>.Instance.CheckIfCanReg(RegLoginType.Mobile, user.mobile);
                if (!checkMobileRes.IsSuccess())
                    return new Resp<long>().WithResp(checkMobileRes);
            }

            user.InitialBaseFromContext();

            return await UserInfoRep.Instance.Add(user);
        }

        /// <summary>
        ///  获取租户平台下的用户列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PageListResp<UserInfoBigMo>> SearchUsers(SearchReq req)
        {
            return await UserInfoRep.Instance.SearchUsers(req);
        }

        /// <summary>
        ///   获取用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<Resp<UserBasicMo>> GetUserById(long userId)
        {
            var getRes = (await UserInfoRep.Instance.GetById(userId));
            return new Resp<UserBasicMo>().WithResp(getRes, UserInfoMoMaps.ConvertToMo);
        }


        public async Task<Resp> ChangeLockStatus(long uId, bool makeLock)
        {
            return await UserInfoRep.Instance.UpdateStatus(uId, makeLock ? UserStatus.Locked : UserStatus.Normal);
        }

    }
}