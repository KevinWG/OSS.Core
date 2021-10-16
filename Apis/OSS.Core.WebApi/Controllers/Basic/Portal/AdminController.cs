#region Copyright (C) 2020 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore 服务层 —— 接口层（前后台用户信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2020-6-1 (儿童节快乐!)
*       
*****************************************************************************/

#endregion

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Resp;
using OSS.Core.RepDapper.Basic.Portal.Mos;
using OSS.Core.Services.Basic.Portal;
using OSS.Core.CoreApi.Controllers.Basic.Portal.Reqs;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Context.Attributes;

namespace OSS.Core.CoreApi.Controllers.Basic.Portal
{
    [ModuleMeta(CoreModuleNames.Portal)]
    [Route("b/[controller]/[action]")]
    public class AdminController : BaseController
    {
        private static readonly AdminService _service = new AdminService();

        /// <summary>
        ///  创建管理员
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFuncMeta(CoreFuncCodes.Portal_Admin_Create)]
        public Task<Resp<long>> Create([FromBody] AddAdminReq req)
        {
            
            if (!ModelState.IsValid)
                return Task.FromResult(new Resp<long>().WithResp(RespTypes.ParaError, GetInvalidMsg()));

            return _service.AddAdmin(req.MapToAdminInfo());
        }

       
        [HttpPost]
        [UserFuncMeta(CoreFuncCodes.Portal_Admin_List)]
        public Task<PageListResp<AdminInfoMo>> SearchAdmins([FromBody]SearchReq req)
        {
            if (req==null)
            {
                return Task.FromResult(new PageListResp<AdminInfoMo>().WithResp(RespTypes.ParaError, "参数不能为空！"));
            }
            return _service.SearchAdmins(req);
        }

        #region 修改管理员信息

        
        /// <summary>
        ///   修改授权管理用户自己的头像地址
        /// </summary>
        /// <param name="avatar"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFuncMeta(CoreFuncCodes.None)]
        public Task<Resp> ChangeOwnerAvatar([FromQuery] string avatar)
        {
            return string.IsNullOrEmpty(avatar) ?
                Task.FromResult(GetInvalidResp())
                : _service.ChangeAvatar(avatar);
        }

        /// <summary>
        ///  锁定用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFuncMeta(CoreFuncCodes.Portal_Admin_Lock)]
        public  Task<Resp> Lock(long uid)
        {
            return uid<=0 
                ? Task.FromResult(GetInvalidResp()) : _service.ChangeLockStatus(uid, true);
        }

        /// <summary>
        ///  解锁用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFuncMeta(CoreFuncCodes.Portal_Admin_UnLock)]
        public  Task<Resp> UnLock(long uid)
        {
            return uid<=0
                ? Task.FromResult(GetInvalidResp()) : _service.ChangeLockStatus(uid, false);
        }

        /// <summary>
        ///  设置管理员类型
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="admin_type"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFuncMeta(CoreFuncCodes.Portal_Admin_SetType)]
        public Task<Resp> SetAdminType(long uid, AdminType admin_type)
        {
            if (uid<=0|| !Enum.IsDefined(typeof(AdminType), admin_type))
            {
                return Task.FromResult(GetInvalidResp());
            }
            return _service.SetAdminType(uid, admin_type);
        }

        #endregion



    }
}
