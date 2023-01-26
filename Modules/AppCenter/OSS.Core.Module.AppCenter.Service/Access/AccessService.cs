using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.AppCenter
{
    /// <summary>
    ///  应用中心服务，平台级应用才能访问
    /// </summary>
    public class AccessService : IAccessService
    {
        private static readonly AccessInfoRep _appRep = new();

        /////<inheritdoc />
        //public Task<Resp<AppInfoMo>> GetAppById(long appid)
        //{
        //    return _appRep.GetById(appid);
        //}
        
        ///<inheritdoc />
        public Task<IResp<AccessInfoMo>> GetAccessByKey(string accessKey, AppPlatform platform)
        {
            return _appRep.GetAppByKey(accessKey, platform);
        }
    }
}