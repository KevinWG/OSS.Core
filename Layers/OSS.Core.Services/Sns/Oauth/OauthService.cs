#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 社会化Oauth授权模块
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-10-14
*       
*****************************************************************************/

#endregion

using OSS.Common.ComModels;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Services.Sns.Exchange;
using OSS.SnsSdk.Oauth.Wx.Mos;

namespace OSS.Core.Services.Sns.Oauth
{

    public class OauthService
    {
        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <param name="plat">平台</param>
        /// <param name="redirectUrl">重定向回跳地址</param>
        /// <param name="state">返回参数，自行编码</param>
        /// <param name="type">授权类型</param>
        /// <returns></returns>
      
        public ResultMo<string> GetOauthUrl(ThirdPaltforms plat, string redirectUrl, string state, AuthClientType type)
        {
            var handler = SnsCommon.GetHandlerByPlatform(plat);
            return handler.GetOauthUrl(redirectUrl, state, type);
        }
        
    }
}
