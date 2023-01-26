
using OSS.Common.Resp;

namespace OSS.Core.Module.Portal
{
    internal interface IAdminCommonService:IAdminOpenService
    {
        /// <summary>
        /// 通过用户Id获取管理员信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal Task<Resp<AdminInfoMo>> GetAdminByUId(long userId);
    }
}
