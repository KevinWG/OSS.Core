using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Helpers;
using OSS.Core.Infrastructure.Web.Attributes.Auth;

namespace OSS.CorePro.TAdminSite.Apis.Portal
{
    /// <summary>
    ///  用户信息模块
    /// </summary>
    public class UserController : ProxyApiController
    {
       

        /// <summary>
        /// 添加创建新用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Portal_UserAdd)]
        public Task<IActionResult> AddUser()
        {
            return PostReqApi("/b/user/AddUser");
        }

        /// <summary>
        /// 获取当前平台的用户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Portal_UserList)]
        public Task<IActionResult> SearchUsers()
        {
            return PostReqApi("/b/user/searchusers");
        }

        /// <summary>
        /// 锁定拉黑当前用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Portal_UserLock)]
        public async Task<Resp> Lock(long id)
        {
            if (id <= 0)
                return new Resp(RespTypes.ParaError, "参数错误！");
            return await RestApiHelper.PostApi<Resp>(string.Concat("/b/user/lock/", id));
        }

        /// <summary>
        /// 解锁用户状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Portal_UserUnLock)]
        public async Task<Resp> UnLock(long id)
        {
            if (id <= 0)
                return new Resp(RespTypes.ParaError, "参数错误！");
            return await RestApiHelper.PostApi<Resp>(string.Concat("/b/user/unlock/", id));
        }
    }
}