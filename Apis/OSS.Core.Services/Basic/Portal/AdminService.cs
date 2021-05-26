#region Copyright (C) 2020 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore 服务层 —— 管理员服务（前后台用户信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2020-6-1 (儿童节快乐!)
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Extension;
using OSS.Common.Helpers;
using OSS.Core.Context;
using OSS.Core.Infrastructure.BasicMos;
using OSS.Core.RepDapper.Basic.Portal;
using OSS.Core.RepDapper.Basic.Portal.Mos;
using OSS.Core.Services.Basic.Portal.IProxies;

namespace OSS.Core.Services.Basic.Portal
{
    /// <summary>
    ///  管理员服务
    /// </summary>
    public class AdminService
    {/// <summary>
    ///  添加管理员
    /// </summary>
    /// <param name="admin"></param>
    /// <returns></returns>
        public async Task<Resp<long>> AddAdmin(AdminInfoMo admin)
        {
            // 判断是否已经绑定
            var exitAdminRes = await AdminInfoRep.Instance.GetAdminByUId(admin.u_id);

            if (exitAdminRes.IsSuccess()) 
                return new Resp<long>().WithResp(RespTypes.ObjectExist, "当前用户已经存在绑定管理员");
            if (!exitAdminRes.IsRespType(RespTypes.ObjectNull))
                return new Resp<long>().WithResp(exitAdminRes);

            // 判断用户本身是否存在问题
            var userRes = await InsContainer<IUserServiceProxy>.Instance.GetUserById(admin.u_id);
            if(!userRes.IsSuccess())
                return new Resp<long>().WithResp(userRes);

            if(userRes.data.status<0)
                return new Resp<long>().WithResp(RespTypes.ObjectExist, "当前绑定用户状态异常！");

            // 执行添加
            admin.InitialBaseFromContext();
            admin.avatar = userRes.data.avatar;
           
            return await AdminInfoRep.Instance.Add(admin);      
        }

        /// <summary>
        ///  管理员查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Task<PageListResp<AdminInfoMo>> SearchAdmins(SearchReq req)
        {
            return AdminInfoRep.Instance.SearchAdmins(req);
        }


        /// <summary>
        ///   修改头像地址
        /// </summary>
        /// <param name="avatar"></param>
        /// <returns></returns>
        public Task<Resp> ChangeAvatar(string avatar)
        {
            return AdminInfoRep.Instance.ChangeAvatar(CoreUserContext.Identity.id.ToInt64(), avatar);
        }


        /// <summary>
        /// 修改锁定状态
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="makeLock"></param>
        /// <returns></returns>
        public Task<Resp> ChangeLockStatus(long uId, bool makeLock)
        {
            return AdminInfoRep.Instance.UpdateStatus(uId, makeLock ? AdminStatus.Locked : AdminStatus.Normal);
        }



        /// <summary>
        /// 修改锁定状态
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="adminType"></param>
        /// <returns></returns>
        public Task<Resp> SetAdminType(long uId, AdminType adminType)
        {
            return AdminInfoRep.Instance.SetAdminType(uId, adminType);
        }



    }
}
