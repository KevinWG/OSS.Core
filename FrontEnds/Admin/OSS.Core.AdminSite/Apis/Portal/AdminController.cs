using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Helpers;
using OSS.Core.Infrastructure.Web.Attributes.Auth;
using OSS.CorePro.TAdminSite.Apis.Portal.Helpers;

namespace OSS.CorePro.TAdminSite.Apis.Portal
{
    /// <summary>
    /// 管理员相关接口访问
    /// </summary>
    public class AdminController:ProxyApiController
    {
        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.None)]
        public async Task<Resp> ChangeOwnerAvatar([FromQuery]string avatar)
        {
            if (string.IsNullOrEmpty(avatar))
                return new Resp(RespTypes.ParaError, "头像信息不能为空！");

            var res = await RestApiHelper.PostApi<Resp>($"/b/admin/changeowneravatar?avatar={avatar}");
            if (res.IsSuccess())
            {
                await AdminHelper.ClearAdminCache();
            }
            return res;
        }


        [HttpPost]
        [UserFuncCode(FuncCodes.Portal_AdminList)]
        public  Task<IActionResult> SearchAdmins()
        {
            return PostReqApi("/b/admin/searchadmins");
        }

        /// <summary>
        /// 锁定拉黑当前用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Portal_AdminLock)]
        public async Task<Resp> Lock(long uid)
        {
            if (uid <= 0)
                return new Resp(RespTypes.ParaError, "参数错误！");
            return await RestApiHelper.PostApi<Resp>(string.Concat("/b/admin/lock?uid=", uid));
        }

        /// <summary>
        /// 解锁用户状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Portal_AdminUnLock)]
        public async Task<Resp> UnLock(long uid)
        {
            if (uid <= 0)
                return new Resp(RespTypes.ParaError, "参数错误！");
            return await RestApiHelper.PostApi<Resp>(string.Concat("/b/admin/unlock?uid=", uid));
        }

        /// <summary>
        /// 解锁用户状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Portal_AdminCreate)]
        public Task<IActionResult> Create()
        {
            return PostReqApi("/b/admin/create");
        }

        /// <summary>
        /// 设置管理员类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Portal_AdminSetType)]
        public Task<IActionResult> SetAdminType(long uid,int admin_type)
        {
            return PostReqApi($"/b/admin/SetAdminType?uid={uid}&admin_type={admin_type}");
        }
    }
}
