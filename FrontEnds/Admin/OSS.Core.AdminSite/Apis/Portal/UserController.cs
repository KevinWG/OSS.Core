using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.BasicMos.Resp;
using OSS.Core.Infrastructure.Const;
using OSS.Core.Infrastructure.Helpers;
using OSS.Core.Infrastructure.Web.Attributes.Auth;
using OSS.CorePro.AdminSite.AppCodes;

namespace OSS.CorePro.TAdminSite.Apis.Portal
{
    /// <summary>
    ///  �û���Ϣģ��
    /// </summary>
    public class UserController : ProxyApiController
    {
       

        /// <summary>
        /// ��Ӵ������û�
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Portal_UserAdd)]
        public Task<IActionResult> AddUser()
        {
            return PostReqApi("/b/user/AddUser");
        }

        /// <summary>
        /// ��ȡ��ǰƽ̨���û��б�
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Portal_UserList)]
        public Task<IActionResult> SearchUsers()
        {
            return PostReqApi("/b/user/searchusers");
        }

        /// <summary>
        /// �������ڵ�ǰ�û�
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Portal_UserLock)]
        public async Task<Resp> Lock(long id)
        {
            if (id <= 0)
                return new Resp(RespTypes.ParaError, "��������");
            return await RestApiHelper.PostApi<Resp>(string.Concat("/b/user/lock/", id));
        }

        /// <summary>
        /// �����û�״̬
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UserFuncCode(FuncCodes.Portal_UserUnLock)]
        public async Task<Resp> UnLock(long id)
        {
            if (id <= 0)
                return new Resp(RespTypes.ParaError, "��������");
            return await RestApiHelper.PostApi<Resp>(string.Concat("/b/user/unlock/", id));
        }
    }
}