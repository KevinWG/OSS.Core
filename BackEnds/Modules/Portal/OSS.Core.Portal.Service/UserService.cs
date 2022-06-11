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
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Domain;
using OSS.Core.Portal.Domain;
using OSS.Core.Portal.Shared.IService;
using OSS.Core.Service;
using OSS.Core.Services.Basic.Portal.Reqs;

namespace OSS.Core.Portal.Service
{
    /// <summary>
    ///  用户服务
    /// </summary>
    public class UserService : BaseService, IUserService
    {
        private static readonly IUserInfoRep _userRep = InsContainer<IUserInfoRep>.Instance;

        ///<inheritdoc/>
        public async Task<Resp<long>> AddUser(UserInfoMo user)
        {
            // todo 修改直接调用 Auth接口 完成逻辑（不需要验证码但判断重复）注册处理
            if (!string.IsNullOrEmpty(user.email))
            {
                var checkEmailRes = await InsContainer<IAuthService>.Instance.CheckIfCanReg(new PortalNameReq()
                {
                    type = PortalType.Email,
                    name = user.email
                });

                if (!checkEmailRes.IsSuccess())
                    return new Resp<long>().WithResp(checkEmailRes);
            }

            if (!string.IsNullOrEmpty(user.mobile))
            {
                var checkMobileRes = await InsContainer<IAuthService>.Instance.CheckIfCanReg(new PortalNameReq()
                {
                    type = PortalType.Mobile,
                    name = user.mobile
                });
                if (!checkMobileRes.IsSuccess())
                    return new Resp<long>().WithResp(checkMobileRes);
            }

            user.FormatBaseByContext();

            return await _userRep.Add(user);
        }

        ///<inheritdoc/>
        public Task<Resp> ChangeMyBasic(UpdateUserBasicReq req)
        {
            var checkRes = ValidateReq(req);

            if (!checkRes.IsSuccess())
                return Task.FromResult(checkRes);
            
            return _userRep.UpdateBasicInfo(CoreContext.User.Identity.id.ToInt64(), req.avatar, req.nick_name);
        }

        ///<inheritdoc/>
        public async Task<PageListResp<UserInfoMo>> SearchUsers(SearchReq req)
        {
            return await _userRep.SearchUsers(req);
        }


        ///<inheritdoc/>
        public Task<Resp<UserBasicMo>> GetMyInfo()
        {
            return GetUserById(CoreContext.User.Identity.id.ToInt64());
        }

        ///<inheritdoc/>
        public async Task<Resp<UserBasicMo>> GetUserById(long userId)
        {
            var getRes = await _userRep.GetById(userId);

            if (!getRes.IsSuccess())
                return new Resp<UserBasicMo>().WithResp(getRes, "获取用户信息失败！");

            getRes.data.pass_word = string.Empty;
            return new Resp<UserBasicMo>(getRes.data);
        }


        ///<inheritdoc/>
        public async Task<Resp> ChangeLockStatus(long uId, bool makeLock)
        {
            return await _userRep.UpdateStatus(uId, makeLock ? UserStatus.Locked : UserStatus.Normal);
        }

    }
}