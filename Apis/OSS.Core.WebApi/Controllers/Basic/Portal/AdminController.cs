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
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Web.Attributes.Auth;
using OSS.Core.RepDapper.Basic.Portal.Mos;
using OSS.Core.Services.Basic.Portal;
using OSS.Core.WebApi.Controllers.Basic.Portal.Reqs;

namespace OSS.Core.WebApi.Controllers.Basic.Portal
{
    [ModuleName(CoreModuleNames.Portal)]
    [Route("b/[controller]/[action]/{id?}")]
    public class AdminController : BaseController
    {
        private static readonly AdminService _service = new AdminService();

        /// <summary>
        ///  创建管理员
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<IdResp<string>> Create([FromBody] AddAdminReq req)
        {
            if (!ModelState.IsValid)
                return Task.FromResult(new IdResp<string>().WithResp(RespTypes.ParaError, GetInvalidMsg()));

            return _service.AddAdmin(req.MapToAdminInfo());
        }

       
        [HttpPost]
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
        public Task<Resp> ChangeOwnerAvatar([FromQuery] string avatar)
        {
            return string.IsNullOrEmpty(avatar) ?
                Task.FromResult(ParaErrorResp)
                : _service.ChangeAvatar(avatar);
        }

        /// <summary>
        ///  锁定用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpPost]
        public  Task<Resp> Lock(string uid)
        {
            return string.IsNullOrEmpty(uid) 
                ? Task.FromResult(ParaErrorResp) : _service.ChangeLockStatus(uid, true);
        }

        /// <summary>
        ///  解锁用户
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpPost]
        public  Task<Resp> UnLock(string uid)
        {
            return string.IsNullOrEmpty(uid)
                ? Task.FromResult(ParaErrorResp) : _service.ChangeLockStatus(uid, false);
        }

        /// <summary>
        ///  设置管理员类型
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="admin_type"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<Resp> SetAdminType(string uid, AdminType admin_type)
        {
            if (string.IsNullOrEmpty(uid)|| !Enum.IsDefined(typeof(AdminType), admin_type))
            {
                return Task.FromResult(ParaErrorResp);
            }
            return _service.SetAdminType(uid, admin_type);
        }

        #endregion



    }
}
