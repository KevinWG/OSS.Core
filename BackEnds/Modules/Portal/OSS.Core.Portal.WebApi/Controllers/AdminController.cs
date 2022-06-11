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

using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Portal.Domain;
using OSS.Core.Portal.Service;
using OSS.Core.Portal.Shared.IService;
using OSS.Core.Portal.WebApi.Controllers;
using System.ComponentModel.DataAnnotations;

namespace OSS.Core.WebApis.Controllers.Basic.Portal
{
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
        public Task<Resp<long>> Create([FromBody] AddAdminReq req)
        {
            return _service.AddAdmin(req.MapToAdminInfo());
        }

        /// <summary>
        /// 管理员列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<PageListResp<AdminInfoMo>> SearchAdmins([FromBody][Required] SearchReq req)
        {
            return _service.SearchAdmins(req);
        }

        #region 修改自己的信息

        /// <summary>
        ///  管理员修改自己的头像地址
        /// </summary>
        /// <param name="avatar"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<Resp> ChangeMyAvatar([FromQuery] string avatar)
        {
            return  _service.ChangeMyAvatar(avatar);
        }

        /// <summary>
        ///   管理员修改自己的名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<Resp> ChangeMyName([FromQuery] string name)
        {
            return  _service.ChangeMyName(name);
        }

        #endregion

        #region 修改其他管理员信息

        /// <summary>
        ///  锁定用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<Resp> Lock(long uid)
        {
            return _service.ChangeLockStatus(uid, true);
        }

        /// <summary>
        ///  解锁用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<Resp> UnLock(long uid)
        {
            return _service.ChangeLockStatus(uid, false);
        }

        /// <summary>
        ///  设置管理员类型
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="admin_type"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<Resp> SetAdminType(long uid, AdminType admin_type)
        {
            return _service.SetAdminType(uid, admin_type);
        }

        #endregion



    }
}
