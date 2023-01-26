using OSS.Common.Resp;
using OSS.Core.Domain;

namespace OSS.Core.Module.AppCenter
{
    public class AccessInfoRep: BaseAppCenterRep<AccessInfoMo>
    {
        public AccessInfoRep() : base(AppCenterConst.RepTables.AccessInfo)
        {
        }
        
        /// <summary>
        ///  通过key和平台查询应用信息
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public Task<IResp<AccessInfoMo>> GetAppByKey(string appKey, AppPlatform platform)
        {
            return Get(a => a.access_key == appKey && a.platform == platform);
        }
    }
}
