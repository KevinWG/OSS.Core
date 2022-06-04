﻿#region Copyright (C) 2020 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore 服务层 —— 管理员服务（前后台用户信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2020-6-1 (儿童节快乐!)
*       
*****************************************************************************/

#endregion

using OSS.Common;
using OSS.Common.Extension;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.Domain.Extension;
using OSS.Core.Portal.Domain;
using OSS.Core.Portal.Shared.IService;

namespace OSS.Core.Services.Basic.Portal
{
    /// <summary>
    ///  管理员服务
    /// </summary>
    public class AdminService
    {
        private static readonly IAdminInfoRep _adminRep = InsContainer<IAdminInfoRep>.Instance;

        #region 管理员修改自己的信息

        /// <summary>
        ///   管理员修改自己头像地址
        /// </summary>
        /// <param name="avatar"></param>
        /// <returns></returns>
        public Task<Resp> ChangeMyAvatar(string avatar)
        {
            return _adminRep.ChangeAvatar(CoreContext.User.Identity.id.ToInt64(), avatar);
        }

        /// <summary>
        ///   管理员修改自己的名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<Resp> ChangeMyName(string name)
        {
            return _adminRep.ChangeMyName(CoreContext.User.Identity.id.ToInt64(), name);
        }


        #endregion
        
        /// <summary>
        ///  添加管理员
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public async Task<Resp<long>> AddAdmin(AdminInfoMo admin)
        {
            // 判断用户本身是否存在问题
            var userRes = await InsContainer<ISharedUserService>.Instance.GetUserById(admin.id);
            if (!userRes.IsSuccess())
                return new Resp<long>().WithResp(userRes);

            if (userRes.data.status < 0)
                return new Resp<long>().WithResp(RespTypes.OperateObjectExist, "当前绑定用户状态异常！");

            // 判断是否已经绑定
            var exitAdminRes = await _adminRep.GetAdminByUId(admin.id);

            if (exitAdminRes.IsSuccess())
                return new Resp<long>().WithResp(RespTypes.OperateObjectExist, "当前用户已经存在绑定管理员");
            if (!exitAdminRes.IsRespType(RespTypes.OperateObjectNull))
                return new Resp<long>().WithResp(exitAdminRes);

            // 执行添加
            admin.InitialBaseFromContext();
            admin.avatar = userRes.data.avatar;

            return await _adminRep.Add(admin);
        }


        /// <summary>
        ///  管理员查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PageListResp<AdminInfoMo>> SearchAdmins(SearchReq req)
        {
            return _adminRep.SearchAdmins(req);
        }
        
        /// <summary>
        /// 修改锁定状态
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="makeLock"></param>
        /// <returns></returns>
        public Task<Resp> ChangeLockStatus(long uId, bool makeLock)
        {
            return _adminRep.UpdateStatus(uId, makeLock ? AdminStatus.Locked : AdminStatus.Normal);
        }

        /// <summary>
        /// 修改锁定状态
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="adminType"></param>
        /// <returns></returns>
        public Task<Resp> SetAdminType(long uId, AdminType adminType)
        {
            return _adminRep.SetAdminType(uId, adminType);
        }
    }
}
