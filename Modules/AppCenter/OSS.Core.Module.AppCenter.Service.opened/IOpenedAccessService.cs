using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.AppCenter
{
    public interface IOpenedAccessService
    {
        /// <summary>
        ///  获取应用信息（通过 平台key + 平台类型
        /// （必须是服务端发起的，AppId不能通过用户交互端传递
        /// </summary>
        /// <param name="appKey">应用key</param>
        /// <param name="platform">应用平台</param>
        /// <returns></returns>
        Task<IResp<AccessInfoMo>> GetAccessByKey(string appKey,AppPlatform platform);

    }
}
