
using OSS.Common.Resp;

namespace OSS.Core.Module.Portal
{
    internal interface IAdminService
    {
        /// <summary>
        /// 通过用户Id获取管理员信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IResp<AdminInfoMo>> GetAdminByUId(long userId);
    }
}
