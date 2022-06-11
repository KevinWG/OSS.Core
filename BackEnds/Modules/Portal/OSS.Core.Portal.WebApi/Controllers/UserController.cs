#region Copyright (C) 2017 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：OSSCore —— 成员相关接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

using Microsoft.AspNetCore.Mvc;
using OSS.Common;
using OSS.Common.Resp;
using OSS.Core.Portal.Domain;
using OSS.Core.Portal.Service;
using OSS.Core.Portal.WebApi.Controllers;
using OSS.Core.Services.Basic.Portal.Reqs;

namespace OSS.Core.WebApis.Controllers.Basic.Portal
{
    /// <summary>
    /// 用户模块
    /// </summary>
    [Route("b/[controller]/[action]")]
    public class UserController : BaseController
    {
        private static readonly UserService _service = new UserService();

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<Resp<long>> AddUser([FromBody] AddUserReq req)
        {
            return _service.AddUser(req.MapToUserInfo());
        }


        /// <summary>
        ///  修改会员自己基础信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<Resp> ChangeMyBasic([FromBody] UpdateUserBasicReq req)
        {
            return _service.ChangeMyBasic(req);
        }

        /// <summary>
        /// 获取当前租户平台下的用户列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageListResp<UserInfoMo>> SearchUsers([FromBody] SearchReq req)
        {
            return await _service.SearchUsers(req);
        }
        
        /// <summary>
        ///  锁定用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Resp> Lock(long id)
        {
            return await _service.ChangeLockStatus(id, true);
        }

        /// <summary>
        ///  解锁用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Resp> UnLock(long id)
        {
            return await _service.ChangeLockStatus(id, false);
        }

        /// <summary>
        ///  获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<Resp<UserBasicMo>> GetUserById(long id)
        {
            return _service.GetUserById(id);
        }

        /// <summary>
        ///  获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<Resp<UserBasicMo>> GetMyInfo()
        {
            return _service.GetMyInfo();
        }
        
    }
}
