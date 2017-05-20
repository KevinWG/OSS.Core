using System.Collections.Generic;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;

namespace OSS.Core.Infrastructure.Utils
{
    public static class ApiSourceKeyUtil
    {
        private static readonly Dictionary<string, string> _appSecretKeys = new Dictionary<string, string>();

        static ApiSourceKeyUtil()
        {
            _appSecretKeys.Add("FrontWeb", "5c567449b8714a038c464059788d4fa6");
        }

        /// <summary>
        /// 获取应用授权key
        /// </summary>
        /// <returns></returns>
        public static ResultMo<string> GetAppSecretKey(string appSource)
        {
            if (_appSecretKeys.ContainsKey(appSource))
            {
               return new ResultMo<string>(_appSecretKeys[appSource]);
            }
            return new ResultMo<string>(ResultTypes.NoRight, "未经授权的应用来源!");
        }
    }
}
