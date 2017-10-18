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

using System.Threading.Tasks;
using OSS.Common.Authrization;
using OSS.Common.ComModels;
using OSS.Core.Domains.Members.Mos;
using OSS.Core.Infrastructure.Enums;
using OSS.Core.Services.Sns.Oauth.Handlers;
using OSS.SnsSdk.Oauth.Wx.Mos;

namespace OSS.Core.Services.Sns.Oauth
{

    public class OauthService
    {
        /// <summary>
        /// 获取授权地址
        /// </summary>
        /// <param name="plat">平台类型</param>
        /// <param name="redirectUrl">回调地址</param>
        /// <param name="state">附加信息，回调时附带</param>
        /// <param name="type">授权类型</param>
        /// <returns>返回授权Url</returns>
        public ResultMo<string> GetOauthUrl( ThirdPaltforms plat,
            string redirectUrl,AuthClientType type, string state = null)
        {
            var handler = GetHandlerByPlatform( plat);
            return handler.GetOauthUrl(redirectUrl, state, type);
        }

        /// <summary>
        /// 注册第三方信息到系统中
        /// </summary>
        /// <param name="plat"></param>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<ResultMo<ThirdPlatformUserMo>> RegisteThirdUser(ThirdPaltforms plat, string code, string state)
        {
            var handler = GetHandlerByPlatform(plat);
            var userInfoRes =await handler.GetOauthUserAsync(code, state);
            //  todo 获取数据库中是否存在第三方用户信息，如果没有添加第三方用户信息到数据库，如果有Update
            return userInfoRes;
        }

        /// <summary>
        /// 获取处理Hander
        /// </summary>
        /// <param name="plat">平台类型</param>
        /// <returns></returns>
        private static BaseOauthHander GetHandlerByPlatform( ThirdPaltforms plat)
        {
            BaseOauthHander handler;
            switch (plat)
            {
                case ThirdPaltforms.Wechat:
                    handler = WxOauthHander.Instance;
                    break;
                //  todo 添加其他平台
                default:
                    handler = BaseOauthHander<BaseOauthHander>.Instance;
                    break;
            }

            handler.SetCOntextConfig(MemberShiper.AppAuthorize);
            return handler;
        }

    }
}
