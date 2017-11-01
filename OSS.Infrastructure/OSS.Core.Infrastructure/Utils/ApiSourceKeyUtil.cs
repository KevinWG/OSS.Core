#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore —— 应用交互Key辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-5-16
*       
*****************************************************************************/

#endregion

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
        public static ResultMo<string> GetAppSecretKey(string appSource, string tenantId=null) 
        {
            return _appSecretKeys.ContainsKey(appSource) 
                ? new ResultMo<string>(_appSecretKeys[appSource]) 
                : new ResultMo<string>(ResultTypes.UnKnowSource, "未知的应用来源!");
        }
    }
}
