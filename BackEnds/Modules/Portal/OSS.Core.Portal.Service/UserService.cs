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
using OSS.Common.Extension;
using OSS.Core.Context;
using OSS.Core.Domain.Extension;
using OSS.Core.Portal.Domain;
using OSS.Core.Portal.Shared.IService;
using OSS.Core.Service;
using OSS.Core.Services.Basic.Portal.Reqs;

namespace OSS.Core.Services.Basic.Portal
{
    /// <summary>
    ///  用户服务
    /// </summary>
    public class UserService : BaseService, ISharedUserService
    {

        private static readonly IUserInfoRep _userRep = InsContainer<IUserInfoRep>.Instance;
        
        /// <summary>
        ///  直接添加用户（管理员权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Resp<long>> AddUser(UserInfoMo user)
        {
            if (!string.IsNullOrEmpty(user.email))
            {
                var checkEmailRes = await InsContainer<ISharedPortalService>.Instance.CheckIfCanReg(new PortalNameReq()
                {
                    type = PortalCodeType.Email,
                    name = user.email
                });
                if (!checkEmailRes.IsSuccess())
                    return new Resp<long>().WithResp(checkEmailRes);
            }

            if (!string.IsNullOrEmpty(user.mobile))
            {
                var checkMobileRes = await InsContainer<ISharedPortalService>.Instance.CheckIfCanReg(new PortalNameReq()
                {
                    type = PortalCodeType.Mobile,
                    name = user.mobile
                });
                if (!checkMobileRes.IsSuccess())
                    return new Resp<long>().WithResp(checkMobileRes);
            }

            user.InitialBaseFromContext();

            return await _userRep.Add(user);
        }

        /// <summary>
        ///  修改基础信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<Resp> ChangeMyBasic(UpdateUserBasicReq req)
        {
            var checkRes = ValidateReq(req);
            if (!checkRes.IsSuccess())
            {
                return Task.FromResult(checkRes);
            }
            return _userRep.UpdateBasicInfo(CoreContext.User.Identity.id.ToInt64(), req.avatar, req.nick_name);
        }

        /// <summary>
        ///  获取租户平台下的用户列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PageListResp<UserInfoMo>> SearchUsers(SearchReq req)
        {
            return await _userRep.SearchUsers(req);
        }


        /// <summary>
        /// 获取我的用户信息
        /// </summary>
        /// <returns></returns>
        public Task<Resp<UserBasicMo>> GetMyInfo()
        {
            return GetUserById(CoreContext.User.Identity.id.ToInt64());
        }

        /// <summary>
        ///   获取用户信息（管理员权限
        /// </summary>
        /// <returns></returns>
        public async Task<Resp<UserBasicMo>> GetUserById(long userId)
        {
            var getRes = await _userRep.GetById(userId);
            return new Resp<UserBasicMo>().WithResp(getRes, UserInfoMoMaps.ConvertToMo, "获取用户信息失败！");
        }

        /// <summary>
        ///  修改锁定状态
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="makeLock"></param>
        /// <returns></returns>
        public async Task<Resp> ChangeLockStatus(long uId, bool makeLock)
        {
            return await _userRep.UpdateStatus(uId, makeLock ? UserStatus.Locked : UserStatus.Normal);
        }

    }
}