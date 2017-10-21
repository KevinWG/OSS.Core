
#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：OSSCore服务层 —— 社交模块通用/对外方法
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-10-21
*       
*****************************************************************************/

#endregion

using OSS.Common.Authrization;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Services.Sns.Oauth.Handlers;

namespace OSS.Core.Services.Sns.Exchange
{
    /// <summary>
    ///  社交模块通用/对外方法
    /// </summary>
    internal  static class SnsCommon
    {
        #region  Oauth模块

        /// <summary>
        /// 获取处理Hander
        /// </summary>
        /// <param name="plat">平台类型</param>
        /// <returns></returns>
        internal static IOauthHander GetHandlerByPlatform(SocialPaltforms plat)
        {
            IOauthHander handler;
            switch (plat)
            {
                case SocialPaltforms.Wechat:
                    handler = WxOauthHander.Instance;
                    break;
                //  todo 添加其他平台
                default:
                    handler = BaseOauthHander<NoneOauthHander>.Instance;
                    break;
            }

            handler.SetCOntextConfig(MemberShiper.AppAuthorize);
            return handler;
        }

        #endregion
    }
}
