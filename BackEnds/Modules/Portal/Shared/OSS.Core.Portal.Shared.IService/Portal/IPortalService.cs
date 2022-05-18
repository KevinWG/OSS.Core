using OSS.Common.Resp;
using OSS.Core.Context;

namespace OSS.Core.Services.Basic.Portal.IProxies
{
    public interface IPortalService
    {
        /// <summary>
        ///  获取登录用户自己信息
        /// </summary>
        /// <returns></returns>
        Task<Resp<UserIdentity>> GetIdentity();

        /// <summary>
        ///  判断是否可以注册
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<Resp> CheckIfCanReg(PortalNameReq req);
    }
}
