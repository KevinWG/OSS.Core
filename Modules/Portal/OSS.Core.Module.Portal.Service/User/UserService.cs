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

namespace OSS.Core.Module.Portal;
    /// <summary>
    ///  用户服务
    /// </summary>
    public class UserService :  IUserService
    {
        private static readonly IUserInfoRep _userRep = InsContainer<IUserInfoRep>.Instance;


        /// <inheritdoc />
        public Task<IResp> ChangeMyBasic(UpdateUserBasicReq req)
        {
            return _userRep.UpdateBasicInfo(CoreContext.User.Identity.id.ToInt64(), req.avatar, req.nick_name);
        }

        /// <inheritdoc />
        public async Task<PageListResp<UserBasicMo>> SearchUsers(SearchReq req)
        {
            return new PageListResp<UserBasicMo>(await  _userRep.SearchUsers(req));
        }



        /// <inheritdoc />
        public async Task<Resp<UserBasicMo>> GetUserById(long userId)
        {
            var getRes = await _userRep.GetById(userId);
            if (!getRes.IsSuccess()) 
                return new Resp<UserBasicMo>().WithResp(getRes, "获取用户信息失败！");

            var userData = getRes.data;
            userData.pass_word = string.Empty;

            return new Resp<UserBasicMo>(userData);
        }




        /// <inheritdoc />
        public async Task<IResp> ChangeLockStatus(long uId, bool makeLock)
        {
            return await _userRep.UpdateStatus(uId, makeLock ? UserStatus.Locked : UserStatus.Normal);
        }
        
    }
