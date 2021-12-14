using System.Threading.Tasks;
using OSS.Common.Resp;
using OSS.Core.Context;
using OSS.Core.RepDapper.Basic.Portal.Mos;

namespace OSS.Core.Services.Basic.Portal.IProxies
{
    public interface IPortalServiceProxy
    {
        /// <summary>
        ///  获取登录用户自己信息
        /// </summary>
        /// <returns></returns>
        Task<Resp<UserIdentity>> GetMyself();

        /// <summary>
        ///  判断是否可以注册
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<Resp> CheckIfCanReg(RegLoginType type, string value);
    }
}
