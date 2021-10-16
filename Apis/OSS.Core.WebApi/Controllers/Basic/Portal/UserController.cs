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

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.BasicMos;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Const;
using OSS.Core.RepDapper.Basic.Portal.Mos; 

using OSS.Core.Services.Basic.Portal;
using OSS.Core.CoreApi.Controllers.Basic.Portal.Reqs;
using OSS.Core.Context.Attributes;

namespace OSS.Core.CoreApi.Controllers.Basic.Portal
{ 
    /// <summary>
    /// 用户模块
    /// </summary>
    [ModuleMeta(CoreModuleNames.Portal)]  
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
        [UserFuncMeta(CoreFuncCodes.Portal_User_Add)]
        public Task<Resp<long>> AddUser([FromBody]AddUserReq req)
        {
            if (string.IsNullOrEmpty(req.email) && string.IsNullOrEmpty(req.mobile) || !ModelState.IsValid)
                return Task.FromResult(GetInvalidResp<long>());

            return _service.AddUser(req.MapToUserInfo());
        }


        /// <summary>
        /// 获取当前租户平台下的用户列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFuncMeta(CoreFuncCodes.Portal_User_List)]
        public async Task<PageListResp<UserInfoBigMo>> SearchUsers([FromBody]SearchReq req)
        {
            if (req == null)
                return new PageListResp<UserInfoBigMo>().WithResp(RespTypes.ParaError, "参数错误");

            return await _service.SearchUsers(req);
        }

        /// <summary>
        ///  锁定用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFuncMeta(CoreFuncCodes.Portal_User_Lock)]
        public async Task<Resp> Lock(long id)
        {
            return await _service.ChangeLockStatus(id,true);
        }

        /// <summary>
        ///  解锁用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFuncMeta(CoreFuncCodes.Portal_User_UnLock)]
        public async Task<Resp> UnLock(long id)
        {
            return await _service.ChangeLockStatus(id,false);
        }

    }
}
