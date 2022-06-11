
using OSS.Common.Resp;
using OSS.Core.Portal.Domain;

namespace OSS.Core.Portal.Service
{
    public interface IAdminService
    {
        /// <summary>
        ///  通过Id获取管理员信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Resp<AdminInfoMo>> GetAdminByUId(long userId);

    }
}
